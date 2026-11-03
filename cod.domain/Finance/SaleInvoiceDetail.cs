using cod.domain.Common;
using cod.domain.Operation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Finance
{
    [Table("saleInvoiceDetails", Schema = "finance")]
    public class SaleInvoiceDetail : Entity
    {
        public int SaleInvoiceId { get; set; }
        [ForeignKey("SaleInvoiceId")]
        public virtual SaleInvoice SaleInvoice { get; set; }
        public int SaleId { get; set; }
        [ForeignKey("SaleId")]
        public virtual Sale Sale { get; set; }
    }
}
