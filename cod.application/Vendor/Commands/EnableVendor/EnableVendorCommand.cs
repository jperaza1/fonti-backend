using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Vendor.Commands.EnableVendor
{
    public class EnableVendorCommand : BaseCommand, IEnableVendorCommand
    {
        public EnableVendorCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(int id)
        {
            var exist = await _repository.AnyAsync<domain.Data.Vendor>(x => x.Id == id);

            if (!exist)
                throw new ApplicationException("PROVEEDOR NO EXISTE", "PROVEEDOR");

            await _repository.UpdateAsync<domain.Data.Vendor>(
                type => type.Id == id,
                type => new domain.Data.Vendor
                {
                    IsActive = true
                });

            await _repository.CommitAsync();
        }
    }
}
