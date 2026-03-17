using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.Sale.Common
{
    public class FilterSaleDto
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int CafeteriaId { get; set; }
        public List<int> Cafeterias { get; set; }
        public SaleType SaleType { get; set; }
        public int ProductTypeId { get; set; }
        public int CategoryId { get; set; }
    }
}
