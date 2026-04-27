using cod.application.Cafeteria.Commands;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.domain.Data;
using cod.domain.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Cafeteria.Common.CreateCafeteria
{
    public class CreateCafeteriaCommand : BaseCommand, ICreateCafeteriaCommand
    {
        private readonly AuthService _authService;
        public CreateCafeteriaCommand(IRepository repository, IExceptionFactory exceptionFactory, AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task Execute(CafeteriaModel model)
        {
            await this.ValidateModel(model);

            var newCafeteria = new domain.Data.Cafeteria
            {
                Name = model.Name,
                Code = model.Code,
                PurchaseOrderCorelative = 1,
                SpecialServicesCorelative = 1,
                OfficeRequisitionCorelative = 1,
                InventoryCorelative = 1,
            };

            var customers = new List<CustomerCafeteria>();

            foreach(var customer in model.Customers)
            {
                customers.Add(new CustomerCafeteria
                {
                    CustomerId = customer.CustomerId,
                    IsActive = true,
                    CreationTime = Clock.Now,
                    CreatorUser = _authService.User
                });
            }

            newCafeteria.CustomerCafeterias = customers;

            _repository.InsertAsync(newCafeteria);

            await _repository.CommitAsync();
        }

        public async Task ValidateModel(CafeteriaModel model)
        {
            if (model.Name.NullOrEmpty())
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Required));

            if (model.Code.NullOrEmpty())
                this._exceptionFactory.Add("code", this._exceptionFactory.GetMessage("Codigo", ExceptionEnums.ExceptionCode.Required));


            bool duplicateName = await _repository
                .AnyAsync<domain.Data.Cafeteria>(f => f.Name.ToUpper() == model.Name.ToUpper());
            if (duplicateName)
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Duplicate));

            bool duplicateCode = await _repository
                .AnyAsync<domain.Data.Cafeteria>(f => f.Code.ToUpper() == model.Code.ToUpper());
            if (duplicateName)
                this._exceptionFactory.Add("code", this._exceptionFactory.GetMessage("Codigo", ExceptionEnums.ExceptionCode.Duplicate));


            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "CAFETERIA", this._exceptionFactory.Exceptions);
        }
    }
}
