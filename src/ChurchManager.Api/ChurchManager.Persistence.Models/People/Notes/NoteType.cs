using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Persistence.Models.People.Notes
{
    [Table("NoteType", Schema = "People")]
    public class NoteType : Entity<int>
    {
        public string Name { get; set; }
        public string CssClass { get; set; }
        public bool IsSystem { get; set; } = false;
    }
}
