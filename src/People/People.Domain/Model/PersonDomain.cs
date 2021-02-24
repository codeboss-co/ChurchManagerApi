using System.Collections.Generic;
using People.Persistence.Models;

namespace People.Domain.Model
{
    public class PersonDomain : Dictionary<string, string>
    {
        public PersonDomain(Person entity) : base(capacity:10)
        {
            Add("PersonId", entity.Id.ToString());

            Add("Title", entity.FullName.Title);
            Add("FirstName", entity.FullName.FirstName);
            Add("MiddleName", entity.FullName.MiddleName);
            Add("LastName", entity.FullName.LastName);
            Add("Suffix", entity.FullName.Suffix);

            Add("BirthDay", entity.BirthDate.BirthDay.ToString());
            Add("BirthMonth", entity.BirthDate.BirthMonth.ToString());
        }
    }
}
