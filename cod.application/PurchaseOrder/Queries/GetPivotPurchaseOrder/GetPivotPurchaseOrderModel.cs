using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.PurchaseOrder.Queries.GetPivotPurchaseOrder
{
    public class GetPivotPurchaseOrderModel
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public DateTime Date { get; set; }
        public string Vendor { get; set; }
        public string Cafeteria { get; set; }  
        public string Product { get; set; }
        public double QuantityRecived { get; set; }
        public double CostRecived { get; set; }
        public double TotalRecived { get; set; }
    }
}
