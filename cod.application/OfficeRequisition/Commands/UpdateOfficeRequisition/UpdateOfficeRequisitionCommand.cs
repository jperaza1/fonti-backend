using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.OfficeRequisition.Common;
using cod.application.SpecialService.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.OfficeRequisition.Commands.UpdateOfficeRequisition
{
    public class UpdateOfficeRequisitionCommand : BaseCommand, IUpdateOfficeRequisitionCommand
    {
        public UpdateOfficeRequisitionCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(OfficeRequisitionModel model, int id)
        {
            ValidateModel(model);

            var officeRequisition = await _repository.Query<domain.Operation.OfficeRequisition>().FirstOrDefaultAsync(f => f.Id == id);

            if (officeRequisition == null)
                throw new ApplicationException("Requisiciones de oficina no existe", "Requisiciones de oficina");


            var details = await _repository.Query<domain.Operation.OfficeRequisitionDetail>().Where(f => f.OfficeRequisitionId == id).ToListAsync();

            var detailsToRemove = details.Where(detail => model.Details.All(member => member.Id != detail.Id)).ToList();

            foreach (var item in detailsToRemove)
            {
                _repository.DeleteSync(item);
            }

            var detailToAdd = model.Details.Where(dd => dd.Id <= 0).ToList();

            foreach (var item in detailToAdd)
            {
                _repository.InsertAsync(new domain.Operation.OfficeRequisitionDetail
                {
                    OfficeRequisitionId = id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                });
            }

            var detailToUpdate = model.Details.Where(dd => dd.Id > 0).ToList();

            foreach (var item in detailToUpdate)
            {
                await _repository.UpdateAsync<domain.Operation.OfficeRequisitionDetail>(
                    registry => registry.Id == item.Id,
                    registry => new domain.Operation.OfficeRequisitionDetail
                    {
                        Quantity = item.Quantity,
                    });
            }

            await _repository.CommitAsync();
        }

        public void ValidateModel(OfficeRequisitionModel model)
        {

            var priceQuantityMismatchError = model.Details.Any(f => f.Quantity == 0);

            if (priceQuantityMismatchError)
                this._exceptionFactory.Add("details", this._exceptionFactory.GetMessage("Cantidad", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "Requisiciones de oficina", this._exceptionFactory.Exceptions);
        }
    }
}
