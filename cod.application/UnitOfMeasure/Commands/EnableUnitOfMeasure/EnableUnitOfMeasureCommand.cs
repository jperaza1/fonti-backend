using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.UnitOfMeasure.Commands.EnableUnitOfMeasure
{
    public class EnableUnitOfMeasureCommand : BaseCommand, IEnableUnitOfMeasureCommand
    {
        public EnableUnitOfMeasureCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(int id)
        {
            var exist = await _repository.AnyAsync<domain.Data.UnitOfMeasure>(x => x.Id == id);
            if (!exist)
                throw new ApplicationException("UNIDAD DE MEDIDA NO EXISTE", "UNIDAD DE MEDIDA");

            await _repository.UpdateAsync<domain.Data.UnitOfMeasure>(
                type => type.Id == id,
                type => new domain.Data.UnitOfMeasure
                {
                    IsActive = true
                });

            await _repository.CommitAsync();
        }
    }
}
