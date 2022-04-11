using Ardalis.Specification;
using ChurchManager.Domain.Common;
using CodeBoss.Extensions;
using System;

namespace ChurchManager.Domain.Features.Groups.Specifications
{
    public class GroupPerformanceMetricsSpecification : Specification<GroupMemberAttendance>
    {
        public GroupPerformanceMetricsSpecification(int groupId, PeriodType periodType)
        {
            Query.AsNoTracking();

            // Group Filter
            Query.Where(g => g.GroupId == groupId);

            // Date Filters
            DateTime from = DateTime.UtcNow;
            DateTime to = DateTime.UtcNow.AddYears(-2);
            switch (periodType)
            {
                case PeriodType.ThisMonth:
                    from = DateTime.UtcNow.StartOfMonth();
                    to = DateTime.UtcNow.EndOfMonth();
                    break;
                case PeriodType.LastMonth:
                    from = DateTime.UtcNow.AddMonths(-1).StartOfMonth();
                    to = DateTime.UtcNow.AddMonths(-1).EndOfMonth();
                    break;
                case PeriodType.ThisYear:
                    from = new DateTime(DateTime.UtcNow.Year, 1, 1);
                    to = DateTime.UtcNow.EndOfMonth();
                    break;
            }

            Query.Where(g => g.AttendanceDate >= from);
            Query.Where(g => g.AttendanceDate <= to);
        }
    }
}
