using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.OfficeRequisition.Common;
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

namespace cod.application.OfficeRequisition.Commands.CreateOfficeRequisition
{
    public class CreateOfficeRequisitionCommand : BaseCommand, ICreateOfficeRequisitionCommand
    {
        private readonly AuthService _authService;
        public CreateOfficeRequisitionCommand(
            IRepository repository, 
            IExceptionFactory exceptionFactory,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task Execute(OfficeRequisitionModel model)
        {
            this.ValidateModel(model);

            var cafeteria = await _repository.QueryActive<domain.Data.Cafeteria>().Where(f => f.Id == model.CafeteriaId).FirstOrDefaultAsync();

            if (cafeteria == null) throw new ApplicationException("Cafeteria no existe", "ORDEN COMPRA");

            var reference = $"{cafeteria.Code}-RE-{cafeteria.OfficeRequisitionCorelative.ToString("D5")}";
            cafeteria.SpecialServicesCorelative++;

            var specialService = new domain.Operation.OfficeRequisition
            {
                CafeteriaId = model.CafeteriaId,
                Date = model.Date,
                Reference = reference,
                Status = DomainEnums.SpecialServiceStatus.Created,
                Description = model.Description,
                OfficeRequisitionDetails = GenerateDetails(model)
            };

            _repository.InsertAsync(specialService);
            _repository.UpdateSync(cafeteria);
            await _repository.CommitAsync();
        }

        public List<domain.Operation.OfficeRequisitionDetail> GenerateDetails(OfficeRequisitionModel model)
        {
            var lst = new List<domain.Operation.OfficeRequisitionDetail>();
            foreach (var item in model.Details)
            {
                var detail = new domain.Operation.OfficeRequisitionDetail
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

        public void ValidateModel(OfficeRequisitionModel model)
        {
            if (model.CafeteriaId <= 0)
                this._exceptionFactory.Add("cafeteria", this._exceptionFactory.GetMessage("Cafeteria", ExceptionEnums.ExceptionCode.Required));

            if(string.IsNullOrEmpty(model.Description))
                this._exceptionFactory.Add("description", this._exceptionFactory.GetMessage("Motivo de la Solicitud", ExceptionEnums.ExceptionCode.Required));

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
