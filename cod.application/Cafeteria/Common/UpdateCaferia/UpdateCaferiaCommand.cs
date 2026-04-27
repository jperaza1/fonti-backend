using cod.application.Cafeteria.Commands;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Type.Commands.UpdateType;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Cafeteria.Common.UpdateCaferia
{
    public class UpdateCaferiaCommand : BaseCommand, IUpdateCaferiaCommand
    {
        public UpdateCaferiaCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(CafeteriaModel model, int id)
        {
            await ValidateModel(model, id);

            await _repository.UpdateAsync<domain.Data.Cafeteria>(
                product => product.Id == id,
                product => new domain.Data.Cafeteria
                {
                    Name = model.Name,
                    Code = model.Code
                });

            var customers = await _repository.Query<domain.Data.CustomerCafeteria>().Where(f => f.CafeteriaId == id).ToListAsync();

            var detailsToRemove = customers.Where(detail => model.Customers.All(member => member.Id != detail.Id)).ToList();

            foreach (var item in detailsToRemove)
            {
                _repository.DeleteSync(item);
            }

            var detailToAdd = model.Customers.Where(dd => dd.Id <= 0).ToList();

            foreach (var item in detailToAdd)
            {
                _repository.InsertAsync(new domain.Data.CustomerCafeteria
                {
                    CustomerId = item.CustomerId,
                    CafeteriaId = id
                });
            }

            await _repository.CommitAsync();
        }

        private async Task ValidateModel(CafeteriaModel model, int id)
        {
            if (model.Name.NullOrEmpty())
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Required));

            if (model.Code.NullOrEmpty())
                this._exceptionFactory.Add("code", this._exceptionFactory.GetMessage("Codigo", ExceptionEnums.ExceptionCode.Required));


            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("Transacción cancelada", "CAFETERIA", this._exceptionFactory.Exceptions);


            bool duplicateName = await _repository
               .AnyAsync<domain.Data.Cafeteria>(registry =>
                   registry.Id != id
                   && registry.Name.ToUpper() == model.Name.ToUpper() && model.Id == registry.Id);

            if (duplicateName)
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Duplicate));


            bool duplicateCode = await _repository
               .AnyAsync<domain.Data.Cafeteria>(registry =>
                   registry.Id != id
                   && registry.Code.ToUpper() == model.Code.ToUpper() && model.Id == registry.Id);

            if (duplicateCode)
                this._exceptionFactory.Add("code", this._exceptionFactory.GetMessage("Codigo", ExceptionEnums.ExceptionCode.Duplicate));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("Transacción cancelada", "CAFETERIA", this._exceptionFactory.Exceptions);


        }
    }
}
