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
                [DomainEnums.UserAlertOption.BirthdaysOfTheWeek] = new UserAlertInfoModel() { Color = "#f27669", TextColor = "white", Icon = "fas fa-birthday-cake", Label = "Cumpleaños de la semana", UserAlert = DomainEnums.UserAlertOption.BirthdaysOfTheWeek },
                [DomainEnums.UserAlertOption.ContractsDueToExpire] = new UserAlertInfoModel() { Color = "#F5B027", TextColor = "white", Icon = "fas fa-hourglass-end", Label = "Contratos por vencer", UserAlert = DomainEnums.UserAlertOption.ContractsDueToExpire },
                
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
