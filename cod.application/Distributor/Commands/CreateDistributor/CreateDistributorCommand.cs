using cod.application.Common.Extensions;
using cod.application.Distributor.Common;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.domain.Features;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Distributor.Commands.CreateDistributor
{
    public class CreateDistributorCommand : BaseCommand, ICreateDistributorCommand
    {
        private readonly AuthService _authService;
        public CreateDistributorCommand(
            IRepository repository,
            IExceptionFactory exceptionFactory,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task Execute(DistributorModel model)
        {
            await ValidateModel(model);

            var newDistributor = new domain.Data.Distributor
            {
                Name = model.Name,
                ProductDistributorPrices = GenerateDetails(model)
            };

            _repository.InsertAsync(newDistributor);

            await _repository.CommitAsync();
        }

        public List<domain.Data.ProductDistributorPrice> GenerateDetails(DistributorModel model)
        {
            var lst = new List<domain.Data.ProductDistributorPrice>();
            if (model.ProductDistributorPrices == null)
                return lst;

            foreach (var item in model.ProductDistributorPrices)
            {
                var detail = new domain.Data.ProductDistributorPrice
                {
                    ProductId = item.ProductId,
                    Price = item.Price,
                    IsActive = true,
                    CreationTime = Clock.Now,
                    CreatorUser = _authService.User,
                };
                lst.Add(detail);
            }
            return lst;
        }

        public async Task ValidateModel(DistributorModel model)
        {
            if (model.Name.NullOrEmpty())
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "DISTRIBUIDOR", this._exceptionFactory.Exceptions);

            bool duplicateName = await _repository
                .AnyAsync<domain.Data.Distributor>(f => f.Name == model.Name);

            if (duplicateName)
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Duplicate));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "DISTRIBUIDOR", this._exceptionFactory.Exceptions);
        }
    }
}
