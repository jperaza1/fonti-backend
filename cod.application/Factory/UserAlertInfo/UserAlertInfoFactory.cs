using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Factory.UserAlertInfo
{
    public class UserAlertInfoFactory : IUserAlertInfoFactory
    {
        private readonly IDictionary<DomainEnums.UserAlertOption, UserAlertInfoModel> alertInfoDictionary;

        public UserAlertInfoFactory()
        {
            alertInfoDictionary = new Dictionary<DomainEnums.UserAlertOption, UserAlertInfoModel>()
            {
                [DomainEnums.UserAlertOption.PendingInvoice] = new UserAlertInfoModel() { Color = "#DC3545", TextColor = "white", Icon = "fas fa-file-invoice", Label = "Facturas Pendientes", UserAlert = DomainEnums.UserAlertOption.PendingInvoice },
                [DomainEnums.UserAlertOption.CorrelativeExpiring] = new UserAlertInfoModel() { Color = "#D97706", TextColor = "white", Icon = "fas fa-receipt", Label = "Correlativos por vencer", UserAlert = DomainEnums.UserAlertOption.CorrelativeExpiring },
                
                
            };
        }

        public List<UserAlertInfoModel> GetInstances(List<DomainEnums.UserAlertOption> alerts)
        {
            var alertsInfo = new List<UserAlertInfoModel>();

            foreach (var alert in alerts)
            {
                if (alertInfoDictionary.ContainsKey(alert))
                    alertsInfo.Add(alertInfoDictionary[alert]);
            }

            return alertsInfo;
        }
    }
}
