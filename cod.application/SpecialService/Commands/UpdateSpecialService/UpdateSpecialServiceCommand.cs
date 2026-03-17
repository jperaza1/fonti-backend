using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.PurchaseOrder.Common;
using cod.application.SpecialService.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.SpecialService.Commands.UpdateSpecialService
{
    public class UpdateSpecialServiceCommand : BaseCommand, IUpdateSpecialServiceCommand
    {
        public UpdateSpecialServiceCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(SpecialServiceModel model, int id)
        {
            ValidateModel(model);

            var specialService = await _repository.Query<domain.Operation.SpecialService>().FirstOrDefaultAsync(f => f.Id == id);

            if (specialService == null)
                throw new ApplicationException("Servicio Especial no existe", "SERVICIO ESPECIAL");


            var details = await _repository.Query<domain.Operation.SpecialServiceDetail>().Where(f => f.SpecialServiceId == id).ToListAsync();

            var detailsToRemove = details.Where(detail => model.Details.All(member => member.Id != detail.Id)).ToList();

            foreach (var item in detailsToRemove)
            {
                _repository.DeleteSync(item);
            }

            var detailToAdd = model.Details.Where(dd => dd.Id <= 0).ToList();

            foreach (var item in detailToAdd)
            {
                _repository.InsertAsync(new domain.Operation.SpecialServiceDetail
                {
                    SpecialServiceId = id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                });
            }

            var detailToUpdate = model.Details.Where(dd => dd.Id > 0).ToList();

            foreach (var item in detailToUpdate)
            {
                await _repository.UpdateAsync<domain.Operation.SpecialServiceDetail>(
                    registry => registry.Id == item.Id,
                    registry => new domain.Operation.SpecialServiceDetail
                    {
                        Quantity = item.Quantity,
                    });
            }

            await _repository.CommitAsync();
        }

        public void ValidateModel(SpecialServiceModel model)
        {

            var priceQuantityMismatchError = model.Details.Any(f => f.Quantity == 0);

            if (priceQuantityMismatchError)
                this._exceptionFactory.Add("details", this._exceptionFactory.GetMessage("Cantidad", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "CAFETERIA", this._exceptionFactory.Exceptions);
        }
    }
}
