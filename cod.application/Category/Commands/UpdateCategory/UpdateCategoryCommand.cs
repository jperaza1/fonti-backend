using cod.application.Common;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Category.Common;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Category.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : BaseCommand, IUpdateCategoryCommand
    {
        private readonly ILogHelper _logHelper;

        public UpdateCategoryCommand(
            IRepository repository,
            IExceptionFactory exceptionFactory,
            ILogHelper logHelper) : base(repository, exceptionFactory)
        {
            _logHelper = logHelper;
        }

        public async Task Execute(CategoryModel model, int id)
        {
            await ValidateModel(model);
            _logHelper.Initialize(_repository);

            var category = await _repository.Query<domain.Data.Category>().Where(f => f.Id == id).FirstOrDefaultAsync();

            if (category == null)
                throw new ApplicationException("Categoría no existe", "CATEGORIA", this._exceptionFactory.Exceptions);

            category.Name = model.Name;

            _repository.UpdateSync(category);

            var details = await _repository.AllActive<domain.Data.SubCategory>().Where(f => f.CategoryId == id).ToListAsync();

            var modelLines = model.SubCategories ?? new List<SubCategoryModel>();

            var detailsToRemove = details.Where(detail => modelLines.All(member => member.Id != detail.Id)).ToList();

            foreach (var item in detailsToRemove)
            {
                _repository.InsertAsync(new domain.General.DataChangeLog
                {
                    Date = Clock.Now,
                    Entity = "CATEGORY",
                    EntityId = id,
                    Log = $"(Eliminando subcategoría) => {item.Name}"
                });
                _repository.DeleteSync(item);
            }

            var detailToAdd = modelLines.Where(dd => dd.Id <= 0).ToList();

            foreach (var item in detailToAdd)
            {
                _repository.InsertAsync(new domain.General.DataChangeLog
                {
                    Date = Clock.Now,
                    Entity = "CATEGORY",
                    EntityId = id,
                    Log = $"(Agregando subcategoría) => {item.Name}"
                });
                _repository.InsertAsync(new domain.Data.SubCategory
                {
                    CategoryId = id,
                    Name = item.Name
                });
            }

            await _repository.CommitAsync();
        }

        public async Task ValidateModel(CategoryModel model)
        {
            if (model.Name.NullOrEmpty())
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "CATEGORIA", this._exceptionFactory.Exceptions);
        }
    }
}
