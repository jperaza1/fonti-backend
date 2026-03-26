using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Sale.Queries.GetSalesByMonth;
using cod.domain.Features.Enums;
using cod.domain.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace cod.application.Sale.Queries.GetDailySalesByCafeteria
{
    public class GetDailySalesByCafeteriaQuery : BaseQuery, IGetDailySalesByCafeteriaQuery
    {
        public GetDailySalesByCafeteriaQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<GetSalesByMonthModel>> Execute()
        {
            var today = Clock.Now;
            var monday = GetMonday(today);

            var baseQuery = _repository.QueryActive<domain.Operation.Sale>()
                .Where(x => x.SaleDate.Date >= monday.Date && x.SaleDate.Date <= today.Date);

            var result = await baseQuery
                .GroupBy(x => new { x.Route.Name, x.SaleDate.Date })
                .Select(g => new GetSalesByMonthModel
                {
                    Route = g.Key.Name,
                    Day = g.Key.Date.ToString("dddd"),
                    Total = Math.Round(g.Sum(f => f.Amount),2),
                }).ToListAsync();

            return result;
        }

        public static DateTime GetMonday(DateTime fecha)
        {
            int diferencia = fecha.DayOfWeek - DayOfWeek.Monday;
            if (diferencia < 0)
                diferencia += 7;

            return fecha.AddDays(-diferencia).Date;
        }
    }
}
