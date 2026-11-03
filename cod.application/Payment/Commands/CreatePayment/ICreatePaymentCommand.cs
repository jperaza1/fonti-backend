using cod.application.Payment.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Payment.Commands.CreatePayment
{
    public interface ICreatePaymentCommand
    {
        Task<int> Execute(PaymentModel model);
    }
}
