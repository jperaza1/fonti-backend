using cod.application.Payment.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Payment.Commands.UpdatePaymentNote
{
    public interface IUpdatePaymentNoteCommand
    {
        Task Execute(int id, PaymentModel model);
    }
}
