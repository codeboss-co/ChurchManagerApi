using Convey.CQRS.Queries;

namespace ChurchManager.Domain.Common.Extensions
{
    public static class PagedQueryExtensions
    {
        public static int DefaultPage => 1;
        public static int DefaultPageSize => 10;

        public static int CalculateTake(int pageSize) => pageSize <= 0 ? DefaultPageSize : pageSize;
        public static int CalculateSkip(int pageSize, int page)
        {
            page = page <= 0 ? DefaultPage : page;

            return CalculateTake(pageSize) * (page - 1);
        }

        public static int CalculateTake(this IPagedQuery paging) => CalculateTake(paging.Results);
        public static int CalculateSkip(this IPagedQuery paging) => CalculateSkip(paging.Results, paging.Page);
    }
}
