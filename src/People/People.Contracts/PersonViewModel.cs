using System.Collections.Generic;
using People.Persistence.Models;

namespace People.Contracts
{
    public class PersonViewModel : Dictionary<string, object>
    {
        public PersonViewModel(Person entity) : base(capacity: 15)
        {
            Add("personId", entity.Id);

            Add("title", entity.FullName.Title);
            Add("firstName", entity.FullName.FirstName);
            Add("middleName", entity.FullName.MiddleName);
            Add("lastName", entity.FullName.LastName);
            Add("suffix", entity.FullName.Suffix);

            Add("photoUrl", entity.PhotoUrl);

            Add("birthDay", entity.BirthDate.BirthDay);
            Add("birthMonth", entity.BirthDate.BirthMonth);
            Add("birthYear", entity.BirthDate.BirthYear);
        }
    }
}
