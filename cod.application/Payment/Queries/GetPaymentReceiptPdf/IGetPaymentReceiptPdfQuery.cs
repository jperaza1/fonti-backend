using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Payment.Queries.GetPaymentReceiptPdf
{
    public interface IGetPaymentReceiptPdfQuery
    {
        Task<string> Execute(int paymentId, int vendorId);
    }
}
