using System;

namespace ChurchManager.Domain.Shared
{
    public record OnlineUserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Status { get; set; }
        public int Unread { get; set; }
        public DateTime LastOnline { get; set; }
    }
}
