using System.Collections.Generic;
using System.Linq;
using People.Persistence.Models;

namespace People.Contracts
{
    public class PersonViewModel : Dictionary<string, object>
    {
        public PersonViewModel(Person entity) : base(capacity: 20)
        {
            Add("personId", entity.Id);

            Add("connectionStatus", entity.ConnectionStatus);
            Add("maritalStatus", entity.MaritalStatus);
            Add("anniversaryDate", entity.AnniversaryDate);

            Add("title", entity.FullName.Title);
            Add("firstName", entity.FullName.FirstName);
            Add("middleName", entity.FullName.MiddleName);
            Add("lastName", entity.FullName.LastName);
            Add("suffix", entity.FullName.Suffix);

            Add("occupation", entity.Occupation);

            Add("ageClassification", entity.AgeClassification);
            Add("gender", entity.Gender);
            Add("receivedHolySpirit", entity.ReceivedHolySpirit);
            Add("baptismStatus", entity.BaptismStatus);
            Add("firstVisitDate", entity.FirstVisitDate);

            Add("photoUrl", entity.PhotoUrl);

            Add("birthDate", entity.BirthDate);

            Add("phoneNumbers", entity.PhoneNumbers);
            Add("email", entity.Email);
            // Family members skipping current person
            Add("familyMembers", entity.Family?.FamilyMembers?.Where(x => x.Id != entity.Id).Select(x => new
            {
                firstName = x.FullName.FirstName,
                middleName = x.FullName.MiddleName,
                lastName = x.FullName.LastName,
                gender = x.Gender,
                ageClassification = x.AgeClassification,
                photoUrl = x.PhotoUrl
            }));
        }
    }
}
