using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Sale.Queries.GetSaleChartReport
{
    public interface IGetSaleChartReportQuery
    {
        Task<List<SaleChartPointModel>> Execute(SaleChartFilterModel filter);
    }
}
