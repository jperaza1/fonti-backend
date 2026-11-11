using cod.application.Features.Models;
using cod.application.Interfaces;
using cod.domain.Features;
using CorrelativeEntity = cod.domain.Data.Correlative;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.Factory.UserAlertData.Implementatios
{
    public class CorrelativeExpiringDataQuery : IUserAlertDataQuery
    {
        private readonly IRepository _repository;

        public CorrelativeExpiringDataQuery(IRepository repository)
        {
            _repository = repository;
        }

        public Task<int> GetCount()
        {
            return Expiring().CountAsync();
        }

        public async Task<AlertPreviewModel> GetPreview()
        {
            var today = Clock.Now.Date;
            var rows = await Expiring()
                .Select(item => new
                {
                    item.Id,
                    item.Cai,
                    item.EmissionPoint,
                    item.Establishment,
                    item.DocumentType,
                    item.Current,
                    item.End,
                    item.EndDate
                })
                .ToListAsync();

            var data = rows
                .Select(item =>
                {
                    var remaining = item.End - item.Current;
                    var days = (int)(item.EndDate.Date - today).TotalDays;
                    var byDate = days <= 30;
                    var byCount = remaining <= 100;
                    var reason = byDate && byCount ? "Fecha y numeración" : byDate ? "Fecha" : "Numeración";
                    return new
                    {
                        item.Id,
                        item.Cai,
                        Point = $"{item.EmissionPoint}-{item.Establishment}-{item.DocumentType}",
                        item.Current,
                        item.End,
                        Remaining = remaining,
                        item.EndDate,
                        RemainingDays = days,
                        Reason = reason
                    };
                })
                .OrderBy(item => item.EndDate)
                .ThenBy(item => item.Remaining)
                .Cast<object>()
                .ToList();

            return new AlertPreviewModel
            {
                Columns = new List<DxColumnModel>
                {
                    new DxColumnModel { DataField = "cai", Caption = "CAI" },
                    new DxColumnModel { DataField = "point", Caption = "Punto" },
                    new DxColumnModel { DataField = "current", Caption = "Actual" },
                    new DxColumnModel { DataField = "end", Caption = "Hasta" },
                    new DxColumnModel { DataField = "remaining", Caption = "Restantes" },
                    new DxColumnModel { DataField = "endDate", Caption = "Fecha límite", CellTemplate = "dateTemplate" },
                    new DxColumnModel { DataField = "remainingDays", Caption = "Días" },
                    new DxColumnModel { DataField = "reason", Caption = "Motivo" }
                },
                DataList = data
            };
        }

        private IQueryable<CorrelativeEntity> Expiring()
        {
            var limit = Clock.Now.Date.AddDays(30);
            return _repository.QueryActive<CorrelativeEntity>()
                .Where(item => item.EndDate.Date <= limit || (item.End - item.Current) <= 100);
        }
    }
}
