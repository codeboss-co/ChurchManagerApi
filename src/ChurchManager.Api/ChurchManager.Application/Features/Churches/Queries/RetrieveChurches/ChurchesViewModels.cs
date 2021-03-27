using System.Collections.Generic;

namespace ChurchManager.Application.Features.Churches.Queries.RetrieveChurches
{
    public record ChurchViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortCode { get; set; }
    }
}
