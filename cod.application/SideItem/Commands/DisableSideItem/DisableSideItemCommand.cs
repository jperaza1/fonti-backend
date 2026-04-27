using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.SideItem.Commands.DisableSideItem
{
    public class DisableSideItemCommand : BaseCommand, IDisableSideItemCommand
    {
        public DisableSideItemCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(int id)
        {
            var exist = await _repository.AnyAsync<domain.SalePoint.SideItem>(x => x.Id == id);

            if (!exist)
                throw new ApplicationException("COMPLEMENTO NO EXISTE", "COMPLEMENTO");

            await _repository.UpdateAsync<domain.SalePoint.SideItem>(
                type => type.Id == id,
                type => new domain.SalePoint.SideItem
                {
                    IsActive = false
                });

            await _repository.CommitAsync();
        }
    }
}
