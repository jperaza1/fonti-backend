using cod.application.PurchaseOrder.Common;
using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.Inventory.Common
{
    public class InventoryModel
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public DateTime Date { get; set; }
        public InventoryType InventoryType { get; set; }
        public string InventoryTypeName { get; set; }
        public string Cafeteria { get; set; }
        public int CafeteriaId { get; set; }
        public double Total { get; set; }
        public bool UseRegister { get; set; }
        public int? RegisterId { get; set; }
        public string Register { get; set; }
        public string CreatorUser { get; set; }
        public string ReasonCanceled { get; set; }
        public string CanceledUser { get; set; }
        public DateTime? CanceledAt { get; set; }
        public List<InventoryDetailModel> Details { get; set; }
    }

    public class InventoryDetailModel
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public string Product { get; set; }
        public int ProductId { get; set; }
        public double Cost { get; set; }
        public double UnitCost { get; set; }
        public double UnitQuantity { get; set; }
        public double QuantityPack { get; set; }
        public double SubTotal { get; set; }
        public double Tax { get; set; }
        public double TaxPercentage { get; set; }
        public double Discount { get; set; }
        public double DiscountPercentage { get; set; }
        public double Total { get; set; }
    }
}
