using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Payment.Commands.GenerateFilePayment
{
    public interface IGenerateFilePaymentCommand
    {
        Task<GenerateFilePaymentModel> Execute(int paymentId);
    }
}
