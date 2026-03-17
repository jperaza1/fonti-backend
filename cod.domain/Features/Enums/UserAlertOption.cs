using System;
using System.Collections.Generic;
using System.Text;

namespace cod.domain.Features.Enums
{
    public partial class DomainEnums
    {
        public enum UserAlertOption
        {
            PendingInvoice = 1,
            PendingPurchaseOrder = 2,
            PendingPurchaseOrderAdmin = 3,
            ApprovedPurchaseOrder = 4,
            ApprovedPurchaseOrderAdmin = 5,
            ProcessingPurchaseOrder = 6,
            ProcessingPurchaseOrderAdmin,
            PendingReviewPurchaseOrder,
            PendingReviewPurchaseOrderAdmin,
            BirthdaysOfTheWeek,
            ContractsDueToExpire,
            TrailPeriodExpire
        }
    }
}
