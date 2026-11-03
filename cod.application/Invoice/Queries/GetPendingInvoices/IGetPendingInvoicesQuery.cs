using cod.application.Invoice.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Invoice.Queries.GetPendingInvoices
{
    public interface IGetPendingInvoicesQuery
    {
        Task<List<InvoiceModel>> Execute(GetPendingInvoicesModel filterModel);
    }
}
