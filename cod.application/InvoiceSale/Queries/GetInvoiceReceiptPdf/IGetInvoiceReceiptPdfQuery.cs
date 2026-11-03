using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.InvoiceSale.Queries.GetInvoiceReceiptPdf
{
    public interface IGetInvoiceReceiptPdfQuery
    {
        Task<string> Execute(int invoiceId);
    }
}
