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
                    Rtn = model.Rtn,
                    Address = model.Address,
                    Phone = model.Phone,
                    Seller = model.Seller,
                    HasCredit = model.HasCredit,
                    CreditDays = model.CreditDays,
                    HasInvoice = model.HasInvoice,
                    RequiresAdminApproval = model.RequiresAdminApproval,
                });

            await _repository.CommitAsync();
        }

        private async Task ValidateModel(VendorModel model, int id)
        {
            if (model.Name.NullOrEmpty())
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Proveedor", ExceptionEnums.ExceptionCode.Required));

            if (model.Rtn.NullOrEmpty())
                this._exceptionFactory.Add("rtn", this._exceptionFactory.GetMessage("Rtn", ExceptionEnums.ExceptionCode.Required));

            if (model.Address.NullOrEmpty())
                this._exceptionFactory.Add("address", this._exceptionFactory.GetMessage("Direccion", ExceptionEnums.ExceptionCode.Required));

            if (model.Phone.NullOrEmpty())
                this._exceptionFactory.Add("phone", this._exceptionFactory.GetMessage("Telefono", ExceptionEnums.ExceptionCode.Required));

            if (model.Seller.NullOrEmpty())
                this._exceptionFactory.Add("seller", this._exceptionFactory.GetMessage("Vendedor", ExceptionEnums.ExceptionCode.Required));

            if (model.HasCredit && model.CreditDays <= 0)
                this._exceptionFactory.Add("creditDays", this._exceptionFactory.GetMessage("Días de crédito", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("Transacción cancelada", "CAFETERIA", this._exceptionFactory.Exceptions);


            bool duplicateName = await _repository
               .AnyAsync<domain.Data.Vendor>(registry =>
                   registry.Id != id
                   && registry.Name.ToUpper() == model.Name.ToUpper() && model.Id == registry.Id);

            if (duplicateName)
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Duplicate));


            bool duplicateRtn = await _repository
               .AnyAsync<domain.Data.Vendor>(registry =>
                   registry.Id != id
                   && registry.Rtn.ToUpper() == model.Rtn.ToUpper() && model.Id == registry.Id);

            if (duplicateRtn)
                this._exceptionFactory.Add("rtn", this._exceptionFactory.GetMessage("Rtn", ExceptionEnums.ExceptionCode.Duplicate));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("Transacción cancelada", "CAFETERIA", this._exceptionFactory.Exceptions);


        }
    }
}
