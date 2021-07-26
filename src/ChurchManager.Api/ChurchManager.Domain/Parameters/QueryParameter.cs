using Convey.CQRS.Queries;

namespace ChurchManager.Domain.Parameters
{
    public record QueryParameter : PagingParameter, IPagedQuery
    {
        public virtual string OrderBy { get; set; }
        public virtual string Fields { get; set; }
        public virtual string SortOrder { get; set; } = "ascending";
    }

    public record SearchTermQueryParameter: QueryParameter
    {
        public string SearchTerm { get; set; }
    }
}
