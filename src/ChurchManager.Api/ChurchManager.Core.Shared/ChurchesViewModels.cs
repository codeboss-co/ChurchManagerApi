namespace ChurchManager.Core.Shared
{
    public record ChurchViewModel : SelectItemViewModel
    {
        public string Description { get; set; }
        public string ShortCode { get; set; }
    }
}
