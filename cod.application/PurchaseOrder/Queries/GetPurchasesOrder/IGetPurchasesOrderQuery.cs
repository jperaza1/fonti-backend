using cod.application.PurchaseOrder.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.PurchaseOrder.Queries.GetPurchasesOrder
{
    public interface IGetPurchasesOrderQuery
    {
        Task<List<PurchaseOrderModel>> Execute(FilterPurchaseOrder filter);
    }
}
