using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Domain.Model.People.Notes
{
    [Table("NoteType")]
    public class NoteType : Entity<int>
    {
        public string Name { get; set; }
        public string CssClass { get; set; }
        public bool IsSystem { get; set; } = false;
    }
}
