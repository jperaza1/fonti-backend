using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Payment.Commands.SendEmailNotification
{
    public interface ISendEmailNotificationCommand
    {
        Task Execute(int paymentId);
    }
}
