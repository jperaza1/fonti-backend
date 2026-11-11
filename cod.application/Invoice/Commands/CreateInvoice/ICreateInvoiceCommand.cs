using cod.application.Invoice.Common;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace cod.application.Invoice.Commands.CreateInvoice
{
    public interface ICreateInvoiceCommand
    {
        Task Execute(InvoiceModel model, IFormFile invoiceFile);
    }
}
