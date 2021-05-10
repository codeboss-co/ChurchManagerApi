using ChurchManager.Domain.Features.People;

namespace ChurchManager.Application.ViewModels
{
    public record PersonBrowseViewModel
    {
        public int PersonId { get; set; }
        public FullName FullName { get; set; }
        public string ConnectionStatus { get; set; }
        public string AgeClassification { get; set; }
        public string Gender { get; set; }
        public string Church { get; set; }
        public BirthDate BirthDate { get; set; }
    }
}
