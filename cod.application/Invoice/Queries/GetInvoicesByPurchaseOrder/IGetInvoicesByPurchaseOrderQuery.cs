using cod.application.Invoice.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Invoice.Queries.GetInvoicesByPurchaseOrder
{
    public interface IGetInvoicesByPurchaseOrderQuery
    {
        Task<List<InvoiceModel>> Execute(int purchaseOrderId);
    }
}
