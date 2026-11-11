using System.Threading.Tasks;

namespace cod.application.Sale.Queries.GetSaleRouteQuantityReport
{
    public interface IGetSaleRouteQuantityReportQuery
    {
        Task<SaleRouteQuantityReportModel> Execute(SaleRouteQuantityFilterModel filter);
    }
}
