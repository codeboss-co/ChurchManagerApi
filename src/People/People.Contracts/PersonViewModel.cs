using System.Collections.Generic;
using System.Linq;
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

            Add("occupation", entity.Occupation);

            Add("ageClassification", entity.AgeClassification);
            Add("gender", entity.Gender);
            Add("receivedHolySpirit", entity.ReceivedHolySpirit);

            Add("photoUrl", entity.PhotoUrl);

            Add("birthDay", entity.BirthDate?.BirthDay);
            Add("birthMonth", entity.BirthDate?.BirthMonth);
            Add("birthYear", entity.BirthDate?.BirthYear);

            Add("phoneNumbers", entity.PhoneNumbers);
            Add("email", entity.Email);
            // Family members skipping current person
            Add("familyMembers", entity.Family?.FamilyMembers?.Where(x => x.Id != entity.Id).Select(x => new
            {
                firstName = x.FullName.FirstName,
                lastName = x.FullName.LastName,
                gender = x.Gender,
                ageClassification = x.AgeClassification,
                photoUrl = x.PhotoUrl
            }));
        }
    }
}
