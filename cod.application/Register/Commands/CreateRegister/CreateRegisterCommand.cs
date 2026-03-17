using cod.application.Cafeteria.Commands;
using cod.application.Common.Extensions;
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

namespace cod.application.Register.Commands.CreateRegister
{
    public class CreateRegisterCommand : BaseCommand, ICreateRegisterCommand
    {
        public CreateRegisterCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(RegisterModel model)
        {
            await this.ValidateModel(model);

            var newRegister = new domain.Data.Register
            {
                Name = model.Name,
                CafeteriaId = model.CafeteriaId,
                UseInInventory = model.UseInInventory,
            };

            _repository.InsertAsync(newRegister);

            await _repository.CommitAsync();
        }

        public async Task ValidateModel(RegisterModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Required));

            if (model.CafeteriaId <= 0)
                this._exceptionFactory.Add("cafeteria", this._exceptionFactory.GetMessage("Cafeteria", ExceptionEnums.ExceptionCode.Required));


            bool duplicateName = await _repository
                .AnyAsync<domain.Data.Register>(f => f.Name.ToUpper() == model.Name.ToUpper() && f.CafeteriaId == model.CafeteriaId);
            if (duplicateName)
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Duplicate));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "CAFETERIA", this._exceptionFactory.Exceptions);
        }
    }
}
