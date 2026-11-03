using cod.application.Invoice.Common;
using cod.application.Payment.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Payment.Commands.AddPayment
{
    public interface IAddPaymentCommand
    {
        Task Execute(InvoicePaymentModel model);
    }
}
