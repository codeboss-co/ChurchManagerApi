using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ChurchManager.Domain.Features.Communication
{
    /// <summary>
    ///     <see href="https://notifications.spec.whatwg.org/#dictdef-notificationoptions">Notification API Standard</see>
    /// </summary>
    [JsonObject("notification")]
    public class PushNotification
    {
        public PushNotification() { }

        public PushNotification(string body)
        {
            Body = body;
        }

        public string Title { get; set; } = "Push Demo";
        public string Lang { get; set; } = "en";
        public string Body { get; set; }
        public string Tag { get; set; }
        public string Image { get; set; }
        public string Icon { get; set; }
        public string Badge { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public bool RequireInteraction { get; set; } = true;
        public IDictionary<string, object> Data { get; set; }
        public List<int> Vibrate { get; set; } = new(0);
        public List<NotificationAction> Actions { get; set; } = new(0);
    }

    /// <summary>
    ///     <see href="https://notifications.spec.whatwg.org/#dictdef-notificationaction">Notification API Standard</see>
    /// </summary>
    public class NotificationAction
    {
        public string Action { get; set; }
        public string Title { get; set; }
        
        public NotificationAction(string action, string title)
        {
            Action = action;
            Title = title;
        }
    }
}
