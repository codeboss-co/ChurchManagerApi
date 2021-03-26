using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Persistence.Models.People.Notes
{
    [Table("Note", Schema = "People")]
    public class Note : AuditableEntity<int>
    {
        public int NoteTypeId { get; set; }

        /// <summary>
        /// Gets or sets the text/body of the note.
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Gets or sets the caption
        /// </summary>
        public string Caption { get; set; }

        public bool IsSystem { get; set; } = false;


        #region Navigation

        public virtual NoteType NoteType { get; set; }

        #endregion
    }
}
