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
                    Name = model.Name.ToUpper(),
                    Email = model.Email,
                    BankId = model.BankId,
                    BankAccount = model.BankAccount,
                    TypeAccountId = model.TypeAccountId,
                    C1 = model.C1,
                    C2 = model.C2,
                    BankAccountHolder = model.BankAccountHolder,
                    IdentificationHolder = model.IdentificationHolder
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

            if (model.BankId.HasValue || !string.IsNullOrEmpty(model.BankAccount) || model.TypeAccountId.HasValue)
            {
                if (string.IsNullOrEmpty(model.BankAccountHolder))
                    this._exceptionFactory.Add("bankAccountHolder", this._exceptionFactory.GetMessage("Titular de la cuenta bancaria", ExceptionEnums.ExceptionCode.Required));

                if (!model.BankId.HasValue || model.BankId.Value <= 0)
                    this._exceptionFactory.Add("bank", this._exceptionFactory.GetMessage("Banco", ExceptionEnums.ExceptionCode.Required));

                if (string.IsNullOrEmpty(model.BankAccount))
                    this._exceptionFactory.Add("bankAccount", this._exceptionFactory.GetMessage("Cuenta de Banco", ExceptionEnums.ExceptionCode.Required));

                if (!model.TypeAccountId.HasValue || model.TypeAccountId.Value <= 0)
                    this._exceptionFactory.Add("typeAccount", this._exceptionFactory.GetMessage("Tipo de Cuenta", ExceptionEnums.ExceptionCode.Required));

                if (!this._exceptionFactory.IsValid())
                    throw new ApplicationException("Transacción cancelada", "VENDEDORES", this._exceptionFactory.Exceptions);

                if (model.Bank == "BANCO FICOHSA" && model.TypeAccount == "CC")
                {
                    if (string.IsNullOrEmpty(model.C1))
                        this._exceptionFactory.Add("c1", this._exceptionFactory.GetMessage("Correlativo #1", ExceptionEnums.ExceptionCode.Required));

                    if (string.IsNullOrEmpty(model.C2))
                        this._exceptionFactory.Add("c2", this._exceptionFactory.GetMessage("Correlativo #2", ExceptionEnums.ExceptionCode.Required));
                }

                if (!this._exceptionFactory.IsValid())
                    throw new ApplicationException("Transacción cancelada", "CAFETERIA", this._exceptionFactory.Exceptions);
            }
        }
    }
}
