using ChurchManager.Domain.Features.People;

namespace ChurchManager.Domain
{
    public record PersonViewModelBasic
    {
        public int PersonId { get; set; }
        public FullName FullName { get; set; }
        public string Gender { get; set; }
        public string AgeClassification { get; set; }
        public string PhotoUrl { get; set; }
        public BirthDate BirthDate { get; set; }
    }
}