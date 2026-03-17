using DocuSign.eSign.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.Sale.Common
{
    public class SaleModel
    {
        public int Id { get; set; }
        public int CafeteriaId { get; set; }
        public string Cafeteria { get; set; }
        public int? RegisterId { get; set; }
        public string Register { get; set; }
        public int? CustomerId { get; set; }
        public string Customer { get; set; }
        public SaleType SaleType { get; set; }
        public string SaleTypeName { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductType { get; set; }
        public int? ServicePeriodId { get; set; }
        public string ServicePeriod { get; set; }
        public DateTime SaleDate { get; set; }
        public double Amount { get; set; }
        public bool IsActive { get; set; }
    }
}
