using ChurchManager.Core.Shared;

namespace ChurchManager.Application.Features.Churches.Queries.RetrieveChurches
{
    public record ChurchViewModel : SelectItemViewModel
    {
        public string Description { get; set; }
        public string ShortCode { get; set; }
    }
}
