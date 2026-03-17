using System.Threading.Tasks;
using cod.application.Features.Exception;
using cod.application.Features.Extensions;
using cod.application.Interfaces;

namespace cod.application.TypeGroup.Commands.UpdateTypeGroup
{
    public class UpdateTypeGroupCommand : IUpdateTypeGroupCommand
    {
        private readonly IRepository _repository;
        private readonly IExceptionFactory _exceptionFactory;

        public UpdateTypeGroupCommand(IRepository repository, IExceptionFactory exceptionFactory)
        {
            _repository = repository;
            _exceptionFactory = exceptionFactory;
        }

        public async Task Execute(UpdateTypeGroupModel updateModel, int id)
        {
            await ValidateModel(updateModel, id);

            await _repository
                .UpdateAsync<domain.Data.TypeGroup>(x => x.Id == id,
                    x => new domain.Data.TypeGroup
                    {
                        Name = updateModel.Name,
                        Code = updateModel.Code
                    });

            await _repository.CommitAsync();
        }

        private async Task ValidateModel(UpdateTypeGroupModel updateModel, int id)
        {
            if (updateModel.Name.NullOrEmpty())
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("name", ExceptionEnums.ExceptionCode.Required));

            if (updateModel.Code.NullOrEmpty())
                this._exceptionFactory.Add("code", this._exceptionFactory.GetMessage("code", ExceptionEnums.ExceptionCode.Required));

            bool duplicateName = await _repository
                .AnyAsync<domain.Data.TypeGroup>(model =>
                    model.Id != id
                    && model.Name.ToUpper() == updateModel.Name.ToUpper());

            if (duplicateName)
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("name", ExceptionEnums.ExceptionCode.Duplicate));


            bool duplicateCode = await _repository
               .AnyAsync<domain.Data.TypeGroup>(model =>
                   model.Id != id
                   && model.Code.ToUpper() == updateModel.Code.ToUpper());

            if (duplicateCode)
                this._exceptionFactory.Add("code", this._exceptionFactory.GetMessage("code", ExceptionEnums.ExceptionCode.Duplicate));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "TYPEGROUP", this._exceptionFactory.Exceptions);
        }
    }
}
