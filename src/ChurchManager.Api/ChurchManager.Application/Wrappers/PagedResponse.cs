using System.Collections.Generic;
using Convey.CQRS.Queries;

namespace ChurchManager.Application.Wrappers
{
    public class PagedResponse<T> : ApiResponse
    {
        public virtual int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long TotalPages { get; set; }
        public long TotalResults { get; set; }

        public PagedResponse(PagedResult<T> pagedResult)
        {
           PageNumber = pagedResult.CurrentPage;
           PageSize = pagedResult.ResultsPerPage;
           TotalPages = pagedResult.TotalPages;
           TotalResults = pagedResult.TotalResults;
           Data = pagedResult.Items;
           Message = null;
           Succeeded = true;
           Errors = null;
        }
    }
}
