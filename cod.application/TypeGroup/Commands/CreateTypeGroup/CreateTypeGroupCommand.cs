using System.Threading.Tasks;
using cod.application.Features.Exception;
using cod.application.Features.Extensions;
using cod.application.Interfaces;

namespace cod.application.TypeGroup.Commands.CreateTypeGroup
{
    public class CreateTypeGroupCommand : ICreateTypeGroupCommand
    {
        private readonly IRepository _repository;
        private readonly IExceptionFactory _exceptionFactory;

        public CreateTypeGroupCommand(IRepository repository, IExceptionFactory exceptionFactory)
        {
            _repository = repository;
            _exceptionFactory = exceptionFactory;
        }

        public async Task Execute(CreateTypeGroupModel createModel)
        {
            await ValidateModel(createModel);

            domain.Data.TypeGroup country = new domain.Data.TypeGroup
            {
                Name = createModel.Name,
                Code = createModel.Code
            };

            _repository.InsertAsync(country);
            await _repository.CommitAsync();

        }

        private async Task ValidateModel(CreateTypeGroupModel createModel)
        {
            if (createModel.Name.NullOrEmpty())
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("name", ExceptionEnums.ExceptionCode.Required));

            if (createModel.Code.NullOrEmpty())
                this._exceptionFactory.Add("code", this._exceptionFactory.GetMessage("code", ExceptionEnums.ExceptionCode.Required));

            bool duplicateName = await _repository
                .AnyAsync<domain.Data.TypeGroup>(model => model.Name.ToUpper() == createModel.Name.ToUpper());
            if (duplicateName)
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("name", ExceptionEnums.ExceptionCode.Duplicate));

            bool duplicateCode = await _repository
             .AnyAsync<domain.Data.TypeGroup>(model => model.Code.ToUpper() == createModel.Code.ToUpper());
            if (duplicateCode)
                this._exceptionFactory.Add("code", this._exceptionFactory.GetMessage("code", ExceptionEnums.ExceptionCode.Duplicate));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "TYPEGROUP", this._exceptionFactory.Exceptions);
        }
    }
}
