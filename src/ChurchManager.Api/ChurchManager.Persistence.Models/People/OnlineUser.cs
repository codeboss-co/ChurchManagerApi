using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Codeboss.Types;

namespace ChurchManager.Persistence.Models.People
{
    [Table("OnlineUser", Schema = "People")]

    public class OnlineUser : IAggregateRoot<int>
    {
        [Key]
        public int Id { get; set; }
        public int PersonId { get; set; }

        [MaxLength(50)]
        public string ConnectionId { get; set; }
        [MaxLength(20)] 
        public string Status { get; set; } = "online"; // offline
        public DateTime? LastOnlineDateTime { get; set; } = DateTime.UtcNow;

        #region Navigation

        public virtual Person Person { get; set; }

        #endregion

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
