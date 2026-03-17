using System.Threading.Tasks;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;

namespace cod.application.Type.Commands.CreateType
{
    public class CreateTypeCommand :BaseCommand,  ICreateTypeCommand
    {
        public CreateTypeCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(CreateTypeModel model)
        {
            await ValidateModel(model);

            domain.Data.Type product = new domain.Data.Type()
            {
                Name = model.Name,
                Code = model.Code,
                TypeGroupId = model.TypeGroupId
            };

            _repository.InsertAsync(product);

            await _repository.CommitAsync();
        }

        async Task ValidateModel(CreateTypeModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
                this._exceptionFactory.Add("name",
                    this._exceptionFactory.GetMessage("name", ExceptionEnums.ExceptionCode.Required));

            if (string.IsNullOrEmpty(model.Code))
                this._exceptionFactory.Add("code",
                    this._exceptionFactory.GetMessage("code", ExceptionEnums.ExceptionCode.Required));

         

            if (model.TypeGroupId <= 0)
                this._exceptionFactory.Add("typeGroupId",
                    this._exceptionFactory.GetMessage("typeGroupId", ExceptionEnums.ExceptionCode.Required));

            
            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("Transaction canceled", "TYPE", this._exceptionFactory.Exceptions);

            bool duplicateName = await _repository
                         .AnyAsync<domain.Data.Type>(registry => registry.Name.ToUpper() == model.Name.ToUpper() && model.TypeGroupId == registry.TypeGroupId );
            if (duplicateName)
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("name", ExceptionEnums.ExceptionCode.Duplicate));

            bool duplicateCode = await _repository
             .AnyAsync<domain.Data.Type>(registry => registry.Code.ToUpper() == model.Code.ToUpper() && model.TypeGroupId == registry.TypeGroupId);
            if (duplicateCode)
                this._exceptionFactory.Add("code", this._exceptionFactory.GetMessage("code", ExceptionEnums.ExceptionCode.Duplicate));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("Transaction canceled", "TYPE", this._exceptionFactory.Exceptions);
        }
    }
}
