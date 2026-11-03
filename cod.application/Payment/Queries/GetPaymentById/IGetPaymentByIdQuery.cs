using cod.application.Payment.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Payment.Queries.GetPaymentById
{
    public interface IGetPaymentByIdQuery
    {
        Task<PaymentModel> Execute(int id);
    }
}
