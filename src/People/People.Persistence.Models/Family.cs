using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Shared.Persistence;

namespace People.Persistence.Models
{
    [Table("Family", Schema = "People")]

    public class Family : Entity<int>
    {
        public string Name { get; set; }

        #region Navigation

        public virtual ICollection<Person> FamilyMembers { get; set; } = new Collection<Person>(); 
        
        #endregion
    }
}
