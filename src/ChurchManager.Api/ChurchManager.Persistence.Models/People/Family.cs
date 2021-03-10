using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Persistence.Models.People
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
