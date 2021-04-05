using System;
using System.ComponentModel.DataAnnotations;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Persistence.Models.People
{
    public class OnlineUser : AuditableEntity<int>, IAggregateRoot<int>
    {
        public string UserLoginId { get; set; }
        [MaxLength(20)] 
        public string Status { get; set; } = "online"; // offline
        public DateTime? LastOnlineDateTime { get; set; } = DateTime.UtcNow;

        public void GoOnline()
        {
            Status = "online";
            LastOnlineDateTime = DateTime.UtcNow;
        }

        public void GoOffline()
        {
            Status = "offline";
            LastOnlineDateTime = DateTime.UtcNow;
        }
    }
}
