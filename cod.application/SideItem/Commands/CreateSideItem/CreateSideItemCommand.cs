using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.SideItem.Common;
using cod.domain.SalePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.SideItem.Commands.CreateSideItem
{
    public class CreateSideItemCommand : BaseCommand, ICreateSideItemCommand
    {
        public CreateSideItemCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(SideItemModel model)
        {
            await ValidateModel(model);

            var newSideItem = new domain.SalePoint.SideItem
            {
                Name = model.Name,
            };

            _repository.InsertAsync(newSideItem);

            await _repository.CommitAsync();
        }

        public async Task ValidateModel(SideItemModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "COMPLEMENTO", this._exceptionFactory.Exceptions);

            bool duplicateName = await _repository
               .AnyAsync<domain.SalePoint.SideItem>(registry => registry.Name.ToUpper() == model.Name.ToUpper());

            if (duplicateName)
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Duplicate));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "COMPLEMENTO", this._exceptionFactory.Exceptions);

        }
    }
}
