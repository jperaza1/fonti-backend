using cod.application.Cafeteria.Commands;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Vendor.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Vendor.Commands.UpdateVendor
{
    public class UpdateVendorCommand : BaseCommand, IUpdateVendorCommand
    {
        public UpdateVendorCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(VendorModel model, int vendorId)
        {
            await ValidateModel(model, vendorId);

            await _repository.UpdateAsync<domain.Data.Vendor>(
                product => product.Id == vendorId,
                product => new domain.Data.Vendor
                {
                    Name = model.Name,
                });

            await _repository.CommitAsync();
        }

        private async Task ValidateModel(VendorModel model, int id)
        {
            if (model.Name.NullOrEmpty())
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Proveedor", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("Transacción cancelada", "CAFETERIA", this._exceptionFactory.Exceptions);


            bool duplicateName = await _repository
               .AnyAsync<domain.Data.Vendor>(registry =>
                   registry.Id != id
                   && registry.Name.ToUpper() == model.Name.ToUpper() && model.Id == registry.Id);

            if (duplicateName)
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Duplicate));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("Transacción cancelada", "CAFETERIA", this._exceptionFactory.Exceptions);
        }
    }
}
