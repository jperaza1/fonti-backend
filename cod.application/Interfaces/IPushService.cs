using System.Threading.Tasks;
using Lib.Net.Http.WebPush;
using cod.application.Features.Models.PushService;

namespace cod.application.Interfaces
{
    public interface IPushService
    {
        Task<VapidPublicKeyModel> GetPublicKey();
        Task InsertUserSubscription(PushSubscription pushSubscription);
        Task SendNotification(PushSubscription pushSubscription, PushMessage message);
        Task Notify(NotificationModel notificationModel);
    }
}
