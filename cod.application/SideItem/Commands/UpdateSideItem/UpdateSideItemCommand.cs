using cod.application.Dish.Common;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.SideItem.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.SideItem.Commands.UpdateSideItem
{
    public class UpdateSideItemCommand : BaseCommand, IUpdateSideItemCommand
    {
        public UpdateSideItemCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(SideItemModel model, int id)
        {
            await ValidateModel(model, id);

            var sideItem = await _repository.Query<domain.SalePoint.SideItem>().Where(f => f.Id == id).FirstOrDefaultAsync();

            if (sideItem == null) throw new ApplicationException("Complemento no existe", "COMPLEMENTO", this._exceptionFactory.Exceptions);

            sideItem.Name = model.Name;

            _repository.UpdateSync(sideItem);

            await _repository.CommitAsync();
        }

        public async Task ValidateModel(SideItemModel model, int id)
        {
            if (string.IsNullOrEmpty(model.Name))
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "COMPLEMENTO", this._exceptionFactory.Exceptions);

            bool duplicateName = await _repository
               .AnyAsync<domain.SalePoint.SideItem>(registry =>
                   registry.Id != id
                   && registry.Name.ToUpper() == model.Name.ToUpper());

            if (duplicateName)
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Duplicate));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "COMPLEMENTO", this._exceptionFactory.Exceptions);

        }
    }
}
