using cod.application.Payment.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Payment.Queries.GetAllPayment
{
    public interface IGetAllPaymentQuery
    {
        Task<List<PaymentModel>> Execute(FilterPaymentModel model);
    }
}
