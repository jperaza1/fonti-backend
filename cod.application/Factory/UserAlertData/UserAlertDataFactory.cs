using cod.application.Factory.UserAlertData.Implementatios;
using cod.application.Interfaces;
using cod.application.Main.Session;
using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Factory.UserAlertData
{
    public class UserAlertDataFactory : IUserAlertDataFactory
    {
        private readonly IRepository _repository;
        private readonly ISessionService _sessionService;
        private readonly IConfigurationService _configurationService;

        public UserAlertDataFactory(IRepository repository,
            ISessionService sessionService,
            IConfigurationService configurationService)
        {
            _repository = repository;
            _sessionService = sessionService;
            _configurationService = configurationService;
        }
        public IUserAlertDataQuery GetInstance(DomainEnums.UserAlertOption alert, bool preview = false)
        {
            switch (alert)
            {
                case DomainEnums.UserAlertOption.PendingInvoice:
                    return new PendingInvoiceDataQuery(_repository);
            }
            return null;
        }
    }
}
