using People.Persistence.Models;
using Shared.Kernel;

namespace People.Domain.Model
{
    public class PersonDomain
    {
        public int PersonId { get; }
        public int? ChurchId { get; }
        public ConnectionStatus ConnectionStatus { get; set; }
        public RecordStatus RecordStatus { get; set; }
        public AgeClassification AgeClassification { get; set; }
        public FullName FullName { get; set; }
        public BirthDate BirthDate { get; set; }

        public PersonDomain(Person entity)
        {
            PersonId = entity.Id;
            ChurchId = entity.ChurchId;
            ConnectionStatus = entity.ConnectionStatus;
            RecordStatus = entity.RecordStatus;
            AgeClassification = entity.AgeClassification;
            FullName = entity.FullName;
            BirthDate = entity.BirthDate;
        }
    }
}
