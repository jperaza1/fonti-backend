using cod.application.PurchaseOrder.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.PurchaseOrder.Queries.GetPurchaseOrder
{
    public interface IGetPurchaseOrderQuery
    {
        Task<PurchaseOrderModel> Execute(int id);
    }
}
