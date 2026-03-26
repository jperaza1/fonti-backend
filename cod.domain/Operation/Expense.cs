using cod.domain.Common;
using cod.domain.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Operation
{
    [Table("Expenses", Schema = "operation")]
    public class Expense : Entity
    {
        public int? InvoiceId { get; set; }
        [ForeignKey("InvoiceId")]
        public virtual Invoice Invoice { get; set; }
        public int? VendorId { get; set; }
        [ForeignKey("VendorId")]
        public virtual Vendor Vendors { get; set; }
        public int RouteId { get; set; }
        [ForeignKey("RouteId")]
        public virtual Route Routes { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public int SubCategoryId { get; set; }
        [ForeignKey("SubCategoryId")]
        public virtual SubCategory SubCateogy { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public bool IsInvoice { get; set; }
        public double Total { get; set; }
    }
}
