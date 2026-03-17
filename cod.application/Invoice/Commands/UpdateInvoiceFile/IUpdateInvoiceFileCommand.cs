using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Invoice.Commands.UpdateInvoiceFile
{
    public interface IUpdateInvoiceFileCommand
    {
        Task Execute(int id, bool status);
    }
}
