using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;
using Task = System.Threading.Tasks.Task;

namespace cod.application.Alerts.Commands.EnableAlertConfiguration
{
    public class EnableAlertConfigurationCommand : BaseCommand, IEnableAlertConfigurationCommand
    {
        public EnableAlertConfigurationCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory) { }
        public async Task Execute(int id)
        {
            var existRegistry = await _repository.AnyAsync<domain.Alerts.AlertConfiguration>(x => x.Id == id);

            if (!existRegistry)
                throw new ApplicationException("ALERT NOT FOUND", "ALERTS");

            await _repository.UpdateAsync<domain.Alerts.AlertConfiguration>(
                registry => registry.Id == id,
                registry => new domain.Alerts.AlertConfiguration
                {
                    IsActive = true
                }
            );
            await _repository.CommitAsync();
        }
    }
}
