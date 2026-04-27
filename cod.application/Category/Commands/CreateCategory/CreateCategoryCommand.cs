using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Category.Common;
using cod.domain.Features;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Category.Commands.CreateCategory
{
    public class CreateCategoryCommand : BaseCommand, ICreateCategoryCommand
    {
        private readonly AuthService _authService;

        public CreateCategoryCommand(
            IRepository repository,
            IExceptionFactory exceptionFactory,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task Execute(CategoryModel model)
        {
            await ValidateModel(model);

            var entity = new domain.Data.Category
            {
                Name = model.Name,
                SubCategories = GenerateDetails(model)
            };

            _repository.InsertAsync(entity);

            await _repository.CommitAsync();
        }

        public List<domain.Data.SubCategory> GenerateDetails(CategoryModel model)
        {
            var list = new List<domain.Data.SubCategory>();
            if (model.SubCategories == null)
                return list;

            foreach (var item in model.SubCategories)
            {
                list.Add(new domain.Data.SubCategory
                {
                    Name = item.Name,
                    InvoiceRequired = item.InvoiceRequired,
                    IsActive = true,
                    CreationTime = Clock.Now,
                    CreatorUser = _authService.User,
                });
            }

            return list;
        }

        public async Task ValidateModel(CategoryModel model)
        {
            if (model.Name.NullOrEmpty())
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "CATEGORIA", this._exceptionFactory.Exceptions);

            bool duplicateName = await _repository
                .AnyAsync<domain.Data.Category>(f => f.Name == model.Name);

            if (duplicateName)
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Duplicate));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "CATEGORIA", this._exceptionFactory.Exceptions);
        }
    }
}
