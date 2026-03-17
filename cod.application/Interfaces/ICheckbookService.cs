using cod.application.Common.CheckBook;
using cod.application.Features.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Interfaces
{
    public interface ICheckbookService 
    {
        Task<ResponseCreateUser> CreateUser(CreateNewUserModel model);

        Task<ResponseCreateZelleAccount> CreateZelleAccount(CreateZelleAccount model, string key, string secret);
        Task<ResponsePaymentResponse> CreatePayment(CreateZellePayment model);
        Task<ResponsePaymentResponse> DepositPayment(string key, string secret, string checkId, string checkBookZelleId);
    }
}
