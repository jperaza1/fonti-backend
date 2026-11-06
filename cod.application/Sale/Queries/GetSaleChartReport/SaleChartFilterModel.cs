using System;

namespace cod.application.Sale.Queries.GetSaleChartReport
{
    public class SaleChartFilterModel
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Report { get; set; }
        public string Product { get; set; }
    }
}
