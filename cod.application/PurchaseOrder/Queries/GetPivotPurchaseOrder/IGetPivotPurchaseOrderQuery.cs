using cod.application.PurchaseOrder.Queries.GetPurchasesOrder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.PurchaseOrder.Queries.GetPivotPurchaseOrder
{
    public interface IGetPivotPurchaseOrderQuery
    {
        Task<List<GetPivotPurchaseOrderModel>> Execute(FilterPurchaseOrder filter);
    }
}
