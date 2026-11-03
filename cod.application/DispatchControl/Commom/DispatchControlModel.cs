using cod.domain.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.DispatchControl.Commom
{
    public class DispatchControlModel
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public int? TruckId { get; set; }
        public string Truck { get; set; }
        public string Route { get; set; }
        public string Vendor { get; set; }
        public DateTime DispatchControlDate { get; set; }
        public DispatchControlStatus Status { get; set; }
        public string StatusName { get; set; }
        public DateTime CreationTime { get; set; }
        public string CreatorUser { get; set; }
        public List<DispatchControlDetailModel> DispatchControlDetails { get; set; }
    }

    public class DispatchControlDetailModel
    {
        public int Id { get; set; }
        public int DispatchControlId { get; set; }
        public DispatchControlType DispatchControlType { get; set; }
        public string DispatchControlTypeName { get; set; }
        public int BottleQuantity { get; set; }
        public int CrateQuantity { get; set; }
        public int MissingBottleQuantity { get; set; }
        public List<ProductMovementModel> ProductMovements { get; set; }
    }

    public class ProductMovementModel
    {
        public int Id { get; set; }
        public int DispatchControlDetailId { get; set; }
        public int ProductId { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
    }

    public class DispatchControlFilterModel
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
