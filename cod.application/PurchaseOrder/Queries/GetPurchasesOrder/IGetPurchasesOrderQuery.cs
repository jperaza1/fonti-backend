using cod.application.PurchaseOrder.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.PurchaseOrder.Queries.GetPurchasesOrder
{
    public interface IGetPurchasesOrderQuery
    {
        Task<List<PurchaseOrderModel>> Execute(FilterPurchaseOrder filter);
    }
}
