using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.domain.Features.Enums;
using cod.domain.Features;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace cod.application.Sale.Queries.GetSalesByMonth
{
    public class GetSaleByMonthQuery : BaseQuery, IGetSaleByMonthQuery
    {
        public GetSaleByMonthQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<GetSalesByMonthModel>> Execute()
        {
            var today = Clock.Now;

            var baseQuery = _repository.QueryActive<domain.Operation.Sale>()
                .Where(x => x.SaleDate.Year == today.Year);

            var result = await baseQuery
                .GroupBy(x => new { x.Route.Name, x.SaleDate.Month })
                .Select(g => new GetSalesByMonthModel
                {
                    Route = g.Key.Name,
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month),
                    Total = Math.Round(g.Sum(f => f.Amount),2 )
                }).ToListAsync();

            return result;
        }
    }
}
