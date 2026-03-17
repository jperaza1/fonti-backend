using cod.application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace cod.infraestructure.Implementations
{
    public class ZonitelService : IZonitelService
    {
        private readonly IConfigurationService _configurationService;
        private string _authToken;
        private string _endpoint;
        private string _phoneNumber;
        private string _userName;
        private string _password;
        private HttpClient _httpClient;

        public ZonitelService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
            _httpClient = new HttpClient();
        }

        private async Task LoadSettingsAsync()
        {
            this._authToken = await this._configurationService.Get<string>("ZONITEL_TOKEN");
            this._phoneNumber = await this._configurationService.Get<string>("ZONITEL_PHONE_NUMBER");
            this._endpoint = await this._configurationService.Get<string>("ZONITEL_URL");
            this._userName = await this._configurationService.Get<string>("ZONITEL_USER_NAME");
            this._password = await this._configurationService.Get<string>("ZONITEL_PASSWORD");
        }

        public async Task SendAsync(string message, string contact)
        {

            await LoadSettingsAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{_userName}:{_password}")));

            var response = await _httpClient.PostAsync($"{_endpoint}", new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "from", _phoneNumber },
                { "to", contact },
                { "text", message },
                { "token", _authToken }
            }));


        }
    }
}
