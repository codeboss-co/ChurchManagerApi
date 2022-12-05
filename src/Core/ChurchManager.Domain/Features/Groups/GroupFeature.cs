using ChurchManager.Persistence.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChurchManager.Domain.Features.Groups
{
    [Table("GroupFeature")]
    public class GroupFeature : Entity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Group> Groups { get; set; }
    }
}
