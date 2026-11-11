using cod.application.Invoice.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Invoice.Queries.GetInvoicesByPurchaseOrder
{
    public interface IGetInvoicesByPurchaseOrderQuery
    {
        Task<List<InvoiceModel>> Execute(int purchaseOrderId);
    }
}
