using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Invoice.Commands.ChangePaymentType
{
    public interface IChangePaymentTypeCommand
    {
        Task Execute(int id);
    }
}
