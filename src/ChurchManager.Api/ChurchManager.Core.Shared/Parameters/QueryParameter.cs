
using Convey.CQRS.Queries;

namespace ChurchManager.Core.Shared.Parameters
{
    public record QueryParameter : PagingParameter, IPagedQuery
    {
        public virtual string OrderBy { get; set; }
        public virtual string Fields { get; set; }
        public virtual string SortOrder { get; set; }
    }

    public record SearchTermQueryParameter: QueryParameter
    {
        public string SearchTerm { get; set; }
    }
}
