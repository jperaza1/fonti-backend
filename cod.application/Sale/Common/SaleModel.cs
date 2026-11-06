using cod.domain.Operation;
using DocuSign.eSign.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.Sale.Common
{
    public class SaleModel
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public string Route { get; set; }
        public string Vendor { get; set; }
        public int? CustomerId { get; set; }
        public int? SaleInvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public string CustomerName { get; set; }
        public SaleType SaleType { get; set; }
        public string SaleTypeName { get; set; }
        public DateTime SaleDate { get; set; }
        public double Amount { get; set; }
        public bool IsActive { get; set; }
        public List<SaleDetailModel> SaleDetails { get; set; }
    }

    public class SaleDetailModel
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public string Product { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }
    }
}
