using cod.application.Common.Extensions;
using cod.application.Features.Models;
using cod.application.Interfaces;
using cod.domain.Features.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Factory.UserAlertData.Implementatios
{
    public class BirthdaysOfTheWeekDataQuery : IUserAlertDataQuery
    {
        private readonly IRepository _repository;

        public BirthdaysOfTheWeekDataQuery(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> GetCount()
        {
            var today = DateTime.Today;

            // Obtener lunes de la semana actual
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
            var endOfWeek = startOfWeek.AddDays(6);

            var result = await _repository.QueryActive<domain.Rrhh.Employee>().Where(f =>
                f.BirthDay.Date.Month == today.Date.Month && f.BirthDay.Date.Day >= startOfWeek.Date.Day && f.BirthDay.Date.Day <= endOfWeek.Date.Day
            ).CountAsync();
            return result;
        }

        public async Task<AlertPreviewModel> GetPreview()
        {
            var today = DateTime.Today;

            // Obtener lunes de la semana actual
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
            var endOfWeek = startOfWeek.AddDays(6);


            var baseQuery = _repository.QueryActive<domain.Rrhh.Employee>().Where(f =>
                f.BirthDay.Date.Month == today.Date.Month && f.BirthDay.Date.Day >= startOfWeek.Date.Day && f.BirthDay.Date.Day <= endOfWeek.Date.Day
            ).Select(f => new
            {
                f.Id,
                Name = $"{f.FirstName} {f.LastName}",
                f.BirthDay,
                Cafeteria = f.Cafeteria.Name,
                Shift = f.Shift.GetEnumDescription(),
                Position = f.Position.Name
            }).OrderBy(f => f.BirthDay);

            List<DxColumnModel> _dxColums1 = new()
            {
                new DxColumnModel{DataField = "name", Caption = "Nombre",  CellTemplate = "nameTemplate" },
                new DxColumnModel{DataField = "birthDay", Caption = "Fecha Cumpleanos",  CellTemplate = "dateTemplate" },
                new DxColumnModel{DataField = "cafeteria", Caption = "Cafeteria" },
                new DxColumnModel{DataField = "shift", Caption = "Turno" },
                new DxColumnModel{DataField = "position", Caption = "Posicion" },
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
