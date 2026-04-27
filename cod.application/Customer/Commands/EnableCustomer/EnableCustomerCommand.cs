using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using Quartz.Impl.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Customer.Commands.EnableCustomer
{
    public class EnableCustomerCommand : BaseCommand, IEnableCustomerCommand
    {
        public EnableCustomerCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(int id)
        {
            var exist = await _repository.AnyAsync<domain.Data.Customer>(x => x.Id == id);

            if (!exist)
                throw new ApplicationException("CLIENTE NO EXISTE", "CLIENTE");

            await _repository.UpdateAsync<domain.Data.Customer>(
                type => type.Id == id,
                type => new domain.Data.Customer
                {
                    IsActive = true
                }
                );

            await _repository.CommitAsync();
        }
    }
}
