using cod.application.Invoice.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Invoice.Queries.GetInvoice
{
    public interface IGetInvoiceQuery
    {
        Task<InvoiceModel> Execute(int id);
    }
}
