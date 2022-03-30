namespace ChurchManager.Domain.Shared
{
    public record SelectItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
