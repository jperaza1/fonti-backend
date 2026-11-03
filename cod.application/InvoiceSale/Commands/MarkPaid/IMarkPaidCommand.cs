using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.InvoiceSale.Commands.MarkPaid
{
    public interface IMarkPaidCommand
    {
        Task Execute(int id);
    }
}
