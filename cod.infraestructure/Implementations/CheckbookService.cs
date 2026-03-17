using cod.application.Common.CheckBook;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DocuSign.eSign.Client.Auth.OAuth.UserInfo;

namespace cod.infraestructure.Implementations
{
    public class CheckbookService : BaseCommand, ICheckbookService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IConfigurationService _configurationService;
        private string publicKey;
        private string secretKey;
        private string checkbookUrl;
        private string cashOnDeliveryAccountId;
        public CheckbookService(
            IRepository repository, 
            IExceptionFactory exceptionFactory,
            IHttpClientService httpClientService,
            IConfigurationService configurationService) : base(repository, exceptionFactory)
        {
            this._httpClientService = httpClientService;
            this._configurationService = configurationService;
        }

        private async Task LoadConfiguration()
        {
            this.secretKey = await _configurationService.Get<string>("CHECKBOOK_SECRET_KEY");
            this.publicKey = await _configurationService.Get<string>("CHECKBOOK_PUBLISH_KEY");
            this.checkbookUrl = await _configurationService.Get<string>("CHECKBOOK_BASE_URL");
            this.cashOnDeliveryAccountId = await _configurationService.Get<string>("CASH_ON_DELIVERY_ACCOUNTID");
        }

        public async Task<ResponseCreateUser> CreateUser(CreateNewUserModel model)
        {
            
            await this.LoadConfiguration();
            this._httpClientService.NewInstance();
            this._httpClientService.AddHeaderAutorization("Authorization", $"{this.publicKey}:{this.secretKey}");
            var response = await this._httpClientService.PostResult<ResponseCreateUser>($"{this.checkbookUrl}/user", model);
            return response;
        }

        public async Task<ResponseCreateZelleAccount> CreateZelleAccount(CreateZelleAccount model, string key, string secret)
        {
            await this.LoadConfiguration();
            this._httpClientService.NewInstance();
            this._httpClientService.AddHeaderAutorization("Authorization", $"{key}:{secret}");
            var response = await this._httpClientService.PostResult<ResponseCreateZelleAccount>($"{this.checkbookUrl}/account/zelle", model);
            return response;
        }

        public async Task<ResponsePaymentResponse> CreatePayment(CreateZellePayment model)
        {
            await this.LoadConfiguration();
            this._httpClientService.NewInstance();
            this._httpClientService.AddHeaderAutorization("Authorization", $"{this.publicKey}:{this.secretKey}");
            model.account = this.cashOnDeliveryAccountId;
            var response = await this._httpClientService.PostResult<ResponsePaymentResponse>($"{this.checkbookUrl}/check/digital", model);
            return response;
        }

        public async Task<ResponsePaymentResponse> DepositPayment(string key, string secret, string checkId, string checkBookZelleId)
        {
            await this.LoadConfiguration();
            this._httpClientService.NewInstance();
            this._httpClientService.AddHeaderAutorization("Authorization", $"{key}:{secret}");
            var model = new
            {
                account = checkBookZelleId
            };
            var response = await this._httpClientService.PostResult<ResponsePaymentResponse>($"{this.checkbookUrl}/check/deposit/{checkId}", model);
            return response;
        }
    }
}
