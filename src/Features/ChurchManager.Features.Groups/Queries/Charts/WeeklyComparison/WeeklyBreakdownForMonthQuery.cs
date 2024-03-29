﻿using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Groups.Queries.Charts.WeeklyComparison
{
    public record WeeklyBreakdownForMonthQuery : IRequest<ApiResponse>
    {
        public int? GroupId { get; set; }
    }

    public class WeeklyBreakdownHandler : IRequestHandler<WeeklyBreakdownForMonthQuery, ApiResponse>
    {
        private readonly IGroupAttendanceDbRepository _dbRepository;

        public WeeklyBreakdownHandler(IGroupAttendanceDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<ApiResponse> Handle(WeeklyBreakdownForMonthQuery query, CancellationToken cancellationToken)
        {
            var results =
                await _dbRepository.WeeklyBreakdownForPeriodAsync(query.GroupId, ReportPeriod.Month, cancellationToken);

            return new ApiResponse(results);
        }
    }
}