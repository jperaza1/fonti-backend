using cod.application.InvoiceSale.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.InvoiceSale.Queries.GetInvoiceSaleById
{
    public interface IGetInvoiceSaleByIdQuery
    {
        Task<SaleInvoiceModel> Execute(int id);
    }
}
