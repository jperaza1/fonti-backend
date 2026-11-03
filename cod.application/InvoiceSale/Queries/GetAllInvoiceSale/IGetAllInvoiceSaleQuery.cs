using cod.application.InvoiceSale.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.InvoiceSale.Queries.GetAllInvoiceSale
{
    public interface IGetAllInvoiceSaleQuery
    {
        Task<List<SaleInvoiceModel>> Execute(FilterInvoiceSaleModel filter);
    }
}
