using System.Collections.Generic;
using cod.domain.Features.Enums;

namespace cod.application.Features.Models.PushService
{
    public class NotificationModel
    {
        public int UserIdentity { get; set; }
        public DomainEnums.UserType UserType { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Action { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }
}
