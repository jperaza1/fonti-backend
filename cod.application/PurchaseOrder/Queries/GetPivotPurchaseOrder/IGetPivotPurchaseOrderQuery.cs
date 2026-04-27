using cod.application.PurchaseOrder.Common;
using cod.application.PurchaseOrder.Queries.GetPurchasesOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.PurchaseOrder.Queries.GetPivotPurchaseOrder
{
    public interface IGetPivotPurchaseOrderQuery
    {
        Task<List<GetPivotPurchaseOrderModel>> Execute(FilterPurchaseOrder filter);
    }
}
