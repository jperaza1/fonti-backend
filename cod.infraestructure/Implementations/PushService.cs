using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Lib.Net.Http.WebPush;
using Lib.Net.Http.WebPush.Authentication;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using cod.application.Features.Extensions;
using cod.application.Features.Models.PushService;
using cod.application.Interfaces;
using cod.domain.Authorization;
using cod.domain.Features;
using cod.domain.Features.Enums;

namespace cod.infraestructure.Implementations
{
    public class PushService : IPushService
    {
        private readonly IConfigurationService _configurationService;
        private readonly IRepository _repository;
        private PushServiceClient _pushClient;

        //private string publicKey = "BJlieW3II-AWxwNp_wXd73N9sDVQMW-8PRaVZoGJALXtLmIgZqISEQPrrzd0NQG4rtYm4zwRRO7y7p4DJbAwRQw";
        //private string privateKey = "I7JWOmhO0LZ7ynbltYhesX-CBA1Y7X6WE8AgTMUeerg";

        private readonly AuthService _authService;
        public PushService(
            IConfigurationService configurationService,
            IRepository repository,
            PushServiceClient pushClient,
            AuthService authService)
        {
            _configurationService = configurationService;
            _repository = repository;
            _pushClient = pushClient;
            _authService = authService;
        }
        public async Task<VapidPublicKeyModel> GetPublicKey()
        {
            string KeyPublic = await _configurationService.Get<string>("PWA_PUBLIC_KEY");
            return new VapidPublicKeyModel
            {
                Key = KeyPublic
            };
        }

        public async Task InsertUserSubscription(PushSubscription pushSubscription)
        {
            _repository.DisableTenantFilter = true;

            var profile = await _repository.FirstOrDefaultAsync<ApplicationUserProfile>(user =>
                user.ApplicationUser.UserName.ToUpper() == _authService.User.ToUpper());
            
            ApplicationUser sosUser = profile?.ApplicationUser;

            _repository.DisableTenantFilter = false;

            ApplicationSubscriptionUser pwaSubscriptionUser = new ApplicationSubscriptionUser
            {
                UserId = sosUser.Id,
                JsonSubscription = JsonConvert.SerializeObject(pushSubscription)
            };
            
            _repository.InsertAsync(pwaSubscriptionUser);
            pwaSubscriptionUser.TenantId = profile.TenantId;
            
            await _repository.CommitAsync();
        }

        public async Task SendNotification(PushSubscription pushSubscription, PushMessage message)
        {
            await _pushClient.RequestPushMessageDeliveryAsync(pushSubscription, message);
        }

        public async Task Notify(NotificationModel notificationModel)
        {

            string publicKey = await _configurationService.Get<string>("PWA_PUBLIC_KEY");

            string privateKey = await _configurationService.Get<string>("PWA_PRIVATE_KEY");

            _pushClient.DefaultAuthentication = new VapidAuthentication(publicKey, privateKey);

            bool notification = await _configurationService.Get<bool>("PROGRESSIVE_WEB_APP_ACTIVE");

            if (!notification)
                return;

            var subscription = await this.GetSubscription(notificationModel);

            if (subscription.IsNull())
                return;

            var messageToSend = this.GetMessageToSend(notificationModel);

            await this.SendNotification(subscription, messageToSend);

        }

        private PushMessage GetMessageToSend(NotificationModel model)
        {
            return new PushNotification
            {
                Title = model.Title,
                Body = model.Body,
                Data = model.Data,
                Actions = new List<PushNotification.NotificationAction>
                {
                    new PushNotification.NotificationAction(model.Action, "View")
                },
                Icon = "assets/img/logo/SOS-logo.png"
            }.ToPushMessage();
        }

        private async Task<PushSubscription> GetSubscription(NotificationModel model)
        {

            var userProfileQuery = _repository
                .Query<domain.Authorization.ApplicationUserProfile>();

            Expression<Func<ApplicationUserProfile, bool>> expressionUserType = profile => true;

            switch (model.UserType)
            {
                case DomainEnums.UserType.General:
                    expressionUserType = profile => profile.Id == model.UserIdentity;
                    break;
            }

            var userWithSubscription = await
                userProfileQuery.Where(expressionUserType)
                    .Select(user => new
                    {
                        user.UserId,
                        Subscription = user.ApplicationUser.ApplicationSubscriptions
                    })
                    .FirstOrDefaultAsync();

            if (userWithSubscription == null)
                return null;

            if (userWithSubscription.Subscription.IsNull() || !userWithSubscription.Subscription.Any())
                return null;

            var subscription = userWithSubscription
                .Subscription
                .LastOrDefault();

            return JsonConvert.DeserializeObject<PushSubscription>(subscription.JsonSubscription);

        }
    }
}
