using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Invoice.Commands.UpdateInvoiceFile
{
    public class UpdateInvoiceFileCommand : BaseCommand, IUpdateInvoiceFileCommand
    {
        private readonly AuthService _authService;

        public UpdateInvoiceFileCommand(
            IRepository repository, 
            IExceptionFactory exceptionFactory,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task Execute(int id, bool status)
        {
            var invoice = await _repository.Query<domain.Operation.Invoice>()
                .Where(f => f.Id == id).FirstOrDefaultAsync();

            if(invoice != null)
            {
                invoice.HasFile = status;
                invoice.FileUploadedAt = status ? DateTime.Now : null;
                invoice.FileUploadedUser = status ? _authService.User : string.Empty;

                _repository.UpdateSync(invoice);

                await _repository.CommitAsync();
            }
        }
    }
}
