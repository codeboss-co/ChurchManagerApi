using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Domain.Features.People
{
    [Table("Family")]

    public class Family : Entity<int>, IAggregateRoot<int>
    {
        public string Name { get; set; }
        public Address Address { get; set; }
        public string Language { get; set; }

        #region Navigation

        public virtual ICollection<Person> FamilyMembers { get; set; } = new Collection<Person>(); 
        
        #endregion
    }

    [Owned]
    public record Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
    }
}
