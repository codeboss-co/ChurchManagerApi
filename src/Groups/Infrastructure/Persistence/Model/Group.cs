
using System.ComponentModel.DataAnnotations;
using Codeboss.Types;

namespace Infrastructure.Persistence.Model
{
    public class Group : IAggregateRoot<int>
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }
    }
}
