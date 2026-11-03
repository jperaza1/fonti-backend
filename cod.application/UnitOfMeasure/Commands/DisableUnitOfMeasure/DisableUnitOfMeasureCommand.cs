using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.UnitOfMeasure.Commands.DisableUnitOfMeasure
{
    public class DisableUnitOfMeasureCommand : BaseCommand, IDisableUnitOfMeasureCommand
    {
        public DisableUnitOfMeasureCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(int id)
        {
            var exist = await _repository.AnyAsync<domain.Data.UnitOfMeasure>(x => x.Id == id);
            if (!exist)
                throw new ApplicationException("UNIDAD DE MEDIDA NO EXISTE", "UNIDAD DE MEDIDA");

            var inUse = await _repository.AnyActive<domain.Data.Product>(x => x.UnitOfMeasureId == id)
                || await _repository.AnyActive<domain.Data.ProductVendor>(x => x.UnitOfMeasureId == id);
            if (inUse)
                throw new ApplicationException("La unidad de medida está asignada a un producto", "UNIDAD DE MEDIDA");

            await _repository.UpdateAsync<domain.Data.UnitOfMeasure>(
                type => type.Id == id,
                type => new domain.Data.UnitOfMeasure
                {
                    IsActive = false
                });

            await _repository.CommitAsync();
        }
    }
}
