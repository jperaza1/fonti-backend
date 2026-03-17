using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.PurchaseOrder.Queries.GetPurchaseOrderVendorPdf
{
    public interface IGetPurchaseOrderVendorPdfQuery
    {
        Task<string> Execute(int id);
    }
}
