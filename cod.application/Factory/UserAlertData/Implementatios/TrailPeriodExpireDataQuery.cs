using cod.application.Common.Extensions;
using cod.application.Features.Models;
using cod.application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Factory.UserAlertData.Implementatios
{
    public class TrailPeriodExpireDataQuery : IUserAlertDataQuery
    {
        private readonly IRepository _repository;

        public TrailPeriodExpireDataQuery(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> GetCount()
        {
            var today = DateTime.Today;
            var expireDate = today.AddDays(7);

            var result = await _repository.QueryActive<domain.Rrhh.Employee>().Where(f =>
                f.EmploymentStatus == domain.Features.Enums.DomainEnums.EmploymentStatusType.Active &&
                f.TrialPeriodDate.HasValue &&
                f.TrialPeriodDate.Value.Date >= today.Date &&
                f.TrialPeriodDate.Value.Date <= expireDate.Date
            ).CountAsync();
            return result;
        }

        public async Task<AlertPreviewModel> GetPreview()
        {
            var today = DateTime.Today;
            var expireDate = today.AddDays(7);


            var baseQuery = _repository.QueryActive<domain.Rrhh.Employee>().Where(f =>
                f.EmploymentStatus == domain.Features.Enums.DomainEnums.EmploymentStatusType.Active &&
                f.TrialPeriodDate.HasValue &&
                f.TrialPeriodDate.Value.Date >= today.Date &&
                f.TrialPeriodDate.Value.Date <= expireDate.Date
            ).Select(f => new
            {
                f.Id,
                Name = $"{f.FirstName} {f.LastName}",
                f.BirthDay,
                Cafeteria = f.Cafeteria.Name,
                Shift = f.Shift.GetEnumDescription(),
                Position = f.Position.Name,
                f.TrialPeriodDate,
                DaysRemaining = (f.TrialPeriodDate.Value.Date - today.Date).TotalDays
            }).OrderBy(f => f.BirthDay);

            List<DxColumnModel> _dxColums1 = new()
            {
                new DxColumnModel{DataField = "name", Caption = "Nombre",  CellTemplate = "nameTemplate" },
                new DxColumnModel{DataField = "cafeteria", Caption = "Cafeteria" },
                new DxColumnModel{DataField = "shift", Caption = "Turno" },
                new DxColumnModel{DataField = "position", Caption = "Posicion" },
                new DxColumnModel{DataField = "trialPeriodDate", Caption = "Fecha Terminacion", CellTemplate= "dateTemplate" },
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
