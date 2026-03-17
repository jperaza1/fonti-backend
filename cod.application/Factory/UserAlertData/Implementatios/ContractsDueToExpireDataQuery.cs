using cod.application.Common.Extensions;
using cod.application.Features.Models;
using cod.application.Interfaces;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Factory.UserAlertData.Implementatios
{
    public class ContractsDueToExpireDataQuery : IUserAlertDataQuery
    {
        private readonly IRepository _repository;

        public ContractsDueToExpireDataQuery(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> GetCount()
        {
            var today = DateTime.Today;
            var expireDate = today.AddDays(60);

            var result = await _repository.QueryActive<domain.Rrhh.Employee>().Where(f =>
                f.EmploymentStatus == domain.Features.Enums.DomainEnums.EmploymentStatusType.Active &&
                f.EmploymentType == domain.Features.Enums.DomainEnums.EmploymentType.Temporary && 
                f.EndDate.HasValue &&
                f.EndDate.Value.Date >= today.Date && 
                f.EndDate.Value.Date <= expireDate.Date
            ).CountAsync();
            return result;
        }

        public async Task<AlertPreviewModel> GetPreview()
        {
            var today = DateTime.Today;
            var expireDate = today.AddDays(60);


            var baseQuery = _repository.QueryActive<domain.Rrhh.Employee>().Where(f =>
                f.EmploymentStatus == domain.Features.Enums.DomainEnums.EmploymentStatusType.Active &&
                f.EmploymentType == domain.Features.Enums.DomainEnums.EmploymentType.Temporary &&
                f.EndDate.HasValue &&
                f.EndDate.Value.Date >= today.Date &&
                f.EndDate.Value.Date <= expireDate.Date
            ).Select(f => new
            {
                f.Id,
                Name = $"{f.FirstName} {f.LastName}",
                f.BirthDay,
                Cafeteria = f.Cafeteria.Name,
                Shift = f.Shift.GetEnumDescription(),
                Position = f.Position.Name,
                f.EndDate,
                DaysRemaining = (f.EndDate.Value.Date - today.Date).TotalDays
            }).OrderBy(f => f.BirthDay);

            List<DxColumnModel> _dxColums1 = new()
            {
                new DxColumnModel{DataField = "name", Caption = "Nombre",  CellTemplate = "nameTemplate" },
                new DxColumnModel{DataField = "cafeteria", Caption = "Cafeteria" },
                new DxColumnModel{DataField = "shift", Caption = "Turno" },
                new DxColumnModel{DataField = "position", Caption = "Posicion" },
                new DxColumnModel{DataField = "endDate", Caption = "Fecha Terminacion", CellTemplate= "dateTemplate" },
                new DxColumnModel{DataField = "daysRemaining", Caption = "Dias Restantes" },
            };

            var result = await baseQuery.ToListAsync();

            return new AlertPreviewModel
            {
                Columns = _dxColums1,
                DataList = result.Cast<object>().ToList()
            };
        }
    }
}
