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

        public PushNotification(string title, string body)
        {
            Title = title;
            Body = body;
        }

        public string Title { get; set; } = "Push Demo";
        public string Lang { get; set; } = "en";
        public string Body { get; set; }
        public string Tag { get; set; }
        /// <summary>
        /// Displaying a large image below the notification's title and message.
        /// Common sizes: 512x256px or 1440x720px
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// Displays to the side of the notification's title and message. Usually the logo:
        /// Recommended: 256x256 or larger
        /// </summary>
        public string Icon { get; set; } = "assets/images/logo/icon.png";
        /// <summary>
        ///  Replaces the Chrome browser icon that appears on the notification tray and above the title
        /// 72x72 or larger
        /// </summary>
        public string Badge { get; set; } = "assets/images/logo/badge.png";
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public bool RequireInteraction { get; set; } = true;
        public IDictionary<string, object> Data { get; set; }
        public List<int> Vibrate { get; set; } = new(3) { 100, 50, 200 };
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
