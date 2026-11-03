using cod.domain.Common;
using cod.domain.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.domain.Operation
{
    [Table("PurchaseOrderDetails", Schema = "operation")]
    public class PurchaseOrderDetail : Entity
    {
        public int PurchaseOrderId { get; set; }
        [ForeignKey("PurchaseOrderId")]
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        public int? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public int? ProductVendorId { get; set; }
        [ForeignKey("ProductVendorId")]
        public virtual ProductVendor ProductVendor { get; set; }
        public double Cost { get; set; }
        public double Quantity { get; set; }
        public double SubTotal { get; set; }
        public double Tax { get; set; }
        public double TaxPercentage { get; set; }
        public double Discount { get; set; }
        public double DiscountPercentage { get; set; }
        public double Total { get; set; }
        public DetailStatus DetailStatus { get; set; }
        public double CostRecived { get; set; }
        public double QuantityRecived { get; set; }
        public double TaxRecived { get; set; }
        public double DiscountRecived { get; set; }
        public double SubTotalRecived { get; set; }
        public double TotalRecived { get; set; }
    }
}
