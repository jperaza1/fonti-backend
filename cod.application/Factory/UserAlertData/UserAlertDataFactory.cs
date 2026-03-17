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
                case DomainEnums.UserAlertOption.PendingPurchaseOrder:
                    return new PendingPurchaseOrderDataQuery(_repository, _sessionService);
                case DomainEnums.UserAlertOption.PendingPurchaseOrderAdmin:
                    return new PendingPurchaseOrderAdminDataQuery(_repository);
                case DomainEnums.UserAlertOption.ApprovedPurchaseOrder:
                    return new ApprovedPurchaseOrderDataQuery(_repository, _sessionService);
                case DomainEnums.UserAlertOption.ApprovedPurchaseOrderAdmin:
                    return new ApprovedPurchaseOrderAdminDataQuery(_repository);
                case DomainEnums.UserAlertOption.ProcessingPurchaseOrder:
                    return new ProcessingPurchaseOrderDataQuery(_repository, _sessionService);
                case DomainEnums.UserAlertOption.ProcessingPurchaseOrderAdmin:
                    return new ProcessingPurchaseOrderAdminDataQuery(_repository);
                case DomainEnums.UserAlertOption.PendingReviewPurchaseOrder:
                    return new PendingReviewPurchaseOrderDataQuery(_repository,_sessionService);
                case DomainEnums.UserAlertOption.PendingReviewPurchaseOrderAdmin:
                    return new PendingReviewPurchaseOrderAdminDataQuery(_repository);
                case DomainEnums.UserAlertOption.BirthdaysOfTheWeek:
                    return new BirthdaysOfTheWeekDataQuery(_repository);
                case DomainEnums.UserAlertOption.ContractsDueToExpire:
                    return new ContractsDueToExpireDataQuery(_repository);
                case DomainEnums.UserAlertOption.TrailPeriodExpire:
                    return new TrailPeriodExpireDataQuery(_repository);
            }
            return null;
        }
    }
}
