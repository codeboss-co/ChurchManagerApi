
using Convey.CQRS.Queries;

namespace ChurchManager.Core.Shared.Parameters
{
    public record QueryParameter : PagingParameter, IPagedQuery
    {
        public virtual string OrderBy { get; set; }
        public virtual string Fields { get; set; }
        public virtual string SortOrder { get; set; }
    }
}
