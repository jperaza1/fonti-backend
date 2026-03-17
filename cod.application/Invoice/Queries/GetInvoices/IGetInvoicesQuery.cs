using cod.application.Invoice.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Invoice.Queries.GetInvoices
{
    public interface IGetInvoicesQuery
    {
        Task<List<InvoiceModel>> Execute(FilterInvoiceModel filterModel);
    }
}
