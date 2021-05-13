using ChurchManager.Domain.Shared;

namespace ChurchManager.Application.ViewModels
{
    public record ChurchViewModel : SelectItemViewModel
    {
        public string Description { get; set; }
        public string ShortCode { get; set; }
    }
}
