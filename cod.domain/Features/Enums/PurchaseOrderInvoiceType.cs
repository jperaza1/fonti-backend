using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Features.Enums
{
    public partial class DomainEnums
    {
        public enum InvoiceType
        {
            [Description("Gasto")]
            Expense = 1,
            [Description("Ingreso")]
            Income,
        }

        public enum PaymentType
        {
            [Description("Contado")]
            Cash = 1,
            [Description("Credito")]
            Credit,
        }

        public enum PaymentStatus
        {
            [Description("Pendiente")]
            Pending = 1,
            [Description("Parcialmente pagado")]
            PartiallyPaid,
            [Description("Pagado")]
            Paid
        }
    }
}
