using System;
using System.Collections.Generic;

namespace cod.application.Sale.Commands.ConsolidateSale
{
    public class ConsolidateSaleModel
    {
        public int RouteId { get; set; }
        public int CustomerId { get; set; }
        public DateTime SaleDate { get; set; }
        public List<int> SaleIds { get; set; }
        public List<ConsolidateSaleLineModel> Lines { get; set; }
    }

    public class ConsolidateSaleLineModel
    {
        public int ProductId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}
