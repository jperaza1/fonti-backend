using cod.application.Main.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Route.Common
{
    public class RouteModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Vendor { get; set; }
        public bool IsActive { get; set; }
        public int? TruckId { get; set; }
        public string Truck { get; set; }
        public int Order { get; set; }
        public bool DeliversRegalia { get; set; }
        public List<ProductRoutePricesModel> ProductRoutePrices { get; set; }
    }

    public class ProductRoutePricesModel
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
    }
}
