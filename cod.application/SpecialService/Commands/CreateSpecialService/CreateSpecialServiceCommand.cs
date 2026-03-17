using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.PurchaseOrder.Common;
using cod.application.SpecialService.Common;
using cod.domain.Features;
using cod.domain.Features.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.SpecialService.Commands.CreateSpecialService
{
    public class CreateSpecialServiceCommand : BaseCommand, ICreateSpecialServiceCommand
    {
        private readonly AuthService _authService;
        public CreateSpecialServiceCommand(
            IRepository repository, 
            IExceptionFactory exceptionFactory,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task Execute(SpecialServiceModel model)
        {
            this.ValidateModel(model);

            var cafeteria = await _repository.QueryActive<domain.Data.Cafeteria>().Where(f => f.Id == model.CafeteriaId).FirstOrDefaultAsync();

            if (cafeteria == null) throw new ApplicationException("Cafeteria no existe", "ORDEN COMPRA");

            var reference = $"{cafeteria.Code}-SE-{cafeteria.SpecialServicesCorelative.ToString("D5")}";
            cafeteria.SpecialServicesCorelative++;

            var specialService = new domain.Operation.SpecialService
            {
                CafeteriaId = model.CafeteriaId,
                Date = model.Date,
                Reference = reference,
                Status = DomainEnums.SpecialServiceStatus.Created,
                SpecialServiceDetails = GenerateDetails(model)
            };

            _repository.InsertAsync(specialService);
            _repository.UpdateSync(cafeteria);
            await _repository.CommitAsync();
        }

        public List<domain.Operation.SpecialServiceDetail> GenerateDetails(SpecialServiceModel model)
        {
            var lst = new List<domain.Operation.SpecialServiceDetail>();
            foreach (var item in model.Details)
            {
                var detail = new domain.Operation.SpecialServiceDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    IsActive = true,
                    CreationTime = Clock.Now,
                    CreatorUser = _authService.User,
                };
                lst.Add(detail);
            }
            return lst;
        }

        public void ValidateModel(SpecialServiceModel model)
        {
            if (model.CafeteriaId <= 0)
                this._exceptionFactory.Add("cafeteria", this._exceptionFactory.GetMessage("Cafeteria", ExceptionEnums.ExceptionCode.Required));

            if (model.Date == DateTime.MinValue)
                this._exceptionFactory.Add("date", this._exceptionFactory.GetMessage("Fecha", ExceptionEnums.ExceptionCode.Required));

            if (model.Details.Count == 0)
                this._exceptionFactory.Add("details", this._exceptionFactory.GetMessage("Producto Requerido", ExceptionEnums.ExceptionCode.Required));

            if (model.Details.Any(f => f.Quantity == 0))
                this._exceptionFactory.Add("details", this._exceptionFactory.GetMessage("Cantidad Requerida", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "CAFETERIA", this._exceptionFactory.Exceptions);
        }
    }
}
