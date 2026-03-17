using cod.application.Features.Models;
using cod.application.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace cod.infraestructure.Implementations
{
    public class TwilioService : ITwilioService
    {
        private readonly IConfigurationService _configurationService;

        public TwilioService(IConfigurationService configurationService)
        {
            this._configurationService = configurationService;
        }

        public async Task SendAsync(TwilioServiceModel model)
        {

            var twilioToken = await _configurationService.Get<string>("TWILIO_TOKEN");
            var twilioSid = await _configurationService.Get<string>("TWILIO_ACCOUNT_SID");
            var twilioPhoneNumber = await _configurationService.Get<string>("TWILIO_PHONE_NUMBER");

            if (string.IsNullOrEmpty(twilioToken) || string.IsNullOrEmpty(twilioSid) || string.IsNullOrEmpty(twilioPhoneNumber))
                return;

            

            TwilioClient.Init(twilioToken, twilioSid);

            await this.Send(twilioPhoneNumber, model.PhoneNumber, model.Body);

        }

        public async Task Send(string twilioPhoneNumber, string phone, string body)
        {
            try
            {
                var toNumber = $"+1{phone.Replace("-", "")}";
                var result = await MessageResource
                        .CreateAsync(
                            body: body,
                            from: new PhoneNumber($"{twilioPhoneNumber}"),
                            to: new PhoneNumber($"{toNumber}")
                        );
            } catch(Exception ex)
            {

            }
        }
    }
}
