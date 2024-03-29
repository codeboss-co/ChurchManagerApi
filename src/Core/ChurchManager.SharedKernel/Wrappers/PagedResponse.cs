﻿using Convey.CQRS.Queries;

namespace ChurchManager.SharedKernel.Wrappers
{
    public class PagedResponse<T> : ApiResponse
    {
        public PagedResponse(PagedResult<T> pagedResult)
        {
            CurrentPage = pagedResult.CurrentPage;
            ResultsPerPage = pagedResult.ResultsPerPage;
            TotalPages = pagedResult.TotalPages;
            TotalResults = pagedResult.TotalResults;
            Data = pagedResult.Items;
            Message = null;
            Succeeded = true;
            Errors = null;
        }

        public virtual int CurrentPage { get; set; }
        public int ResultsPerPage { get; set; }
        public long TotalPages { get; set; }
        public long TotalResults { get; set; }
    }
}