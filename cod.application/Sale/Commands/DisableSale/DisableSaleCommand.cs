using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Sale.Commands.DisableSale
{
    public class DisableSaleCommand : BaseCommand, IDisableSaleCommand
    {
        public DisableSaleCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(int id)
        {
            var exist = await _repository.AnyAsync<domain.Operation.Sale>(x => x.Id == id);

            if (!exist)
                throw new ApplicationException("VENTA NO EXISTE", "VENTa");

            await _repository.UpdateAsync<domain.Operation.Sale>(
                type => type.Id == id,
                type => new domain.Operation.Sale
                {
                    IsActive = false
                });

            await _repository.CommitAsync();
        }
    }
}
