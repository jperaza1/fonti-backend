using System.Collections.Generic;

namespace cod.application.Sale.Queries.GetSaleRouteQuantityReport
{
    public class SaleRouteQuantityReportModel
    {
        public List<SaleRouteQuantityProductModel> Products { get; set; } = new List<SaleRouteQuantityProductModel>();
        public List<SaleRouteQuantityRowModel> Rows { get; set; } = new List<SaleRouteQuantityRowModel>();
        public List<int> Totals { get; set; } = new List<int>();
    }

    public class SaleRouteQuantityProductModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
    }

    public class SaleRouteQuantityRowModel
    {
        public int RouteId { get; set; }
        public string Route { get; set; }
        public string Vendor { get; set; }
        public List<int> Quantities { get; set; } = new List<int>();
    }
}
