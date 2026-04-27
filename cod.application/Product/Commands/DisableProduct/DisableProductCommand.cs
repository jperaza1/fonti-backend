using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Product.Commands.DisableProduct
{
    public class DisableProductCommand : BaseCommand, IDisableProductCommand
    {
        public DisableProductCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(int id)
        {
            var exist = await _repository.AnyAsync<domain.Data.Product>(x => x.Id == id);

            if (!exist)
                throw new ApplicationException("PRODUCTO NO EXISTE", "PRODUCTO");

            await _repository.UpdateAsync<domain.Data.Product>(
                type => type.Id == id,
                type => new domain.Data.Product
                {
                    IsActive = false
                });

            await _repository.CommitAsync();
        }
    }
}
