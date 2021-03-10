using ChurchManager.Core.Shared.Parameters;

namespace ChurchManager.Api.Requests
{
    public record SearchTermRequest : QueryParameter
    {
        public string SearchTerm { get; set; }
    }
}
