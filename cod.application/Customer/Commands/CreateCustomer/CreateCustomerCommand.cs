using cod.application.Customer.Common;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Register.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Customer.Commands.CreateCustomer
{
    public class CreateCustomerCommand : BaseCommand, ICreateCustomerCommand
    {
        public CreateCustomerCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(CustomerModel model)
        {
            await this.ValidateModel(model);

            var newRegister = new domain.Data.Customer
            {
                Name = model.Name,
                Rtn = model.Rtn,
            };

            _repository.InsertAsync(newRegister);

            await _repository.CommitAsync();
        }

        public async Task ValidateModel(CustomerModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Required));

            if (string.IsNullOrEmpty(model.Rtn))
                this._exceptionFactory.Add("rtn", this._exceptionFactory.GetMessage("Rtn", ExceptionEnums.ExceptionCode.Required));

            bool duplicateName = await _repository
                .AnyAsync<domain.Data.Customer>(f => f.Name.ToUpper() == model.Name.ToUpper());
            if (duplicateName)
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Duplicate));

            bool duplicateRtn = await _repository
                .AnyAsync<domain.Data.Customer>(f => f.Rtn.ToUpper() == model.Rtn.ToUpper());
            if (duplicateName)
                this._exceptionFactory.Add("rtn", this._exceptionFactory.GetMessage("Rtn", ExceptionEnums.ExceptionCode.Duplicate));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "CLIENTE", this._exceptionFactory.Exceptions);
        }
    }
}
