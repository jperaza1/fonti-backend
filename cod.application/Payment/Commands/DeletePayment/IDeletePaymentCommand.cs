using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Payment.Commands.DeletePayment
{
    public interface IDeletePaymentCommand
    {
        Task Execute(int id);
    }
}
