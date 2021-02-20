using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Codeboss.Types;

namespace People.Infrastructure.Persistence.Model
{
    public class Family : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        public string RecordStatus { get; set; }

        public string Name { get; set; }

        #region Navigation

        public virtual ICollection<Person> FamilyMembers { get; set; } = new Collection<Person>(); 
        
        #endregion
    }
}
