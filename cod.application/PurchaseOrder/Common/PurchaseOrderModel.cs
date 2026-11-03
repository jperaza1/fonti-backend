using System;
using System.Collections.Generic;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.PurchaseOrder.Common
{
    public class PurchaseOrderModel
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public DateTime Date { get; set; }
        public string Vendor { get; set; }
        public int VendorId { get; set; }
        public double Total { get; set; }
        public string OrderStatusName { get; set; }
        public string ApprovedUser { get; set; }
        public string CreatorUser { get; set; }
        public bool HasInvoice { get; set; }
        public string Reason { get; set; }
        public string ReasonCanceled { get; set; }
        public string CanceledUser { get; set; }
        public DateTime? CanceledAt { get; set; }
        public PurchaseOrderStatus OrderStatus { get; set; }
        public List<PurchaseOrderDetailModel> Details { get; set; }
        public string TypeInoice { get; set; }
    }

    public class PurchaseOrderDetailModel
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public string Product { get; set; }
        public int? ProductId { get; set; }
        public int? ProductVendorId { get; set; }
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
