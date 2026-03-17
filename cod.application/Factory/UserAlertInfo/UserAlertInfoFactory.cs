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
                [DomainEnums.UserAlertOption.PendingPurchaseOrder] = new UserAlertInfoModel() { Color = "#FD7E14", TextColor = "white", Icon = "fas fa-file", Label = "OC Pendiente Aprobación", UserAlert = DomainEnums.UserAlertOption.PendingPurchaseOrder },
                [DomainEnums.UserAlertOption.PendingPurchaseOrderAdmin] = new UserAlertInfoModel() { Color = "#FD7E14", TextColor = "white", Icon = "fas fa-file", Label = "OC Pendiente Aprobación", UserAlert = DomainEnums.UserAlertOption.PendingPurchaseOrderAdmin },
                [DomainEnums.UserAlertOption.ApprovedPurchaseOrder] = new UserAlertInfoModel() { Color = "#28A745", TextColor = "white", Icon = "fas fa-file", Label = "OC Aprobadas", UserAlert = DomainEnums.UserAlertOption.ApprovedPurchaseOrder },
                [DomainEnums.UserAlertOption.ApprovedPurchaseOrderAdmin] = new UserAlertInfoModel() { Color = "#28A745", TextColor = "white", Icon = "fas fa-file", Label = "OC Aprobadas", UserAlert = DomainEnums.UserAlertOption.ApprovedPurchaseOrderAdmin },
                [DomainEnums.UserAlertOption.ProcessingPurchaseOrder] = new UserAlertInfoModel() { Color = "#17A2B8", TextColor = "white", Icon = "fas fa-file", Label = "OC en proceso", UserAlert = DomainEnums.UserAlertOption.ProcessingPurchaseOrder },
                [DomainEnums.UserAlertOption.ProcessingPurchaseOrderAdmin] = new UserAlertInfoModel() { Color = "#17A2B8", TextColor = "white", Icon = "fas fa-file", Label = "OC en proceso", UserAlert = DomainEnums.UserAlertOption.ProcessingPurchaseOrderAdmin },
                [DomainEnums.UserAlertOption.PendingReviewPurchaseOrder] = new UserAlertInfoModel() { Color = "#FFC107", TextColor = "white", Icon = "fas fa-file", Label = "OC Pendiente de Revisión", UserAlert = DomainEnums.UserAlertOption.PendingReviewPurchaseOrder },
                [DomainEnums.UserAlertOption.PendingReviewPurchaseOrderAdmin] = new UserAlertInfoModel() { Color = "#FFC107", TextColor = "white", Icon = "fas fa-file", Label = "OC Pendiente de Revisión", UserAlert = DomainEnums.UserAlertOption.PendingReviewPurchaseOrderAdmin },
                [DomainEnums.UserAlertOption.BirthdaysOfTheWeek] = new UserAlertInfoModel() { Color = "#f27669", TextColor = "white", Icon = "fas fa-birthday-cake", Label = "Cumpleaños de la semana", UserAlert = DomainEnums.UserAlertOption.BirthdaysOfTheWeek },
                [DomainEnums.UserAlertOption.ContractsDueToExpire] = new UserAlertInfoModel() { Color = "#F5B027", TextColor = "white", Icon = "fas fa-hourglass-end", Label = "Contratos por vencer", UserAlert = DomainEnums.UserAlertOption.ContractsDueToExpire },
                [DomainEnums.UserAlertOption.TrailPeriodExpire] = new UserAlertInfoModel() { Color = "#F5B027", TextColor = "white", Icon = "fas fa-hourglass-end", Label = "Periodo de Prueba por vencer", UserAlert = DomainEnums.UserAlertOption.TrailPeriodExpire },
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
