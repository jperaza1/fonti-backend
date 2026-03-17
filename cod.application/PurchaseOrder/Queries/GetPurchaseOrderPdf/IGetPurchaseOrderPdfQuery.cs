using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.PurchaseOrder.Queries.GetPurchaseOrderPdf
{
    public interface IGetPurchaseOrderPdfQuery
    {
        Task<string> Execute(int id);
    }
}
