using Ardalis.Specification;
using ChurchManager.Domain.Common;
using CodeBoss.Extensions;
using System;

namespace ChurchManager.Domain.Features.Groups.Specifications
{
    public class AttendanceReportSubmissionsSpecification : Specification<GroupAttendance, int>
    {
        public AttendanceReportSubmissionsSpecification(int groupTypeId, PeriodType periodType)
        {
            Query.AsNoTracking();


            // Group Type Filter
            Query.Where(g => g.Group.GroupTypeId == groupTypeId);
            
           
            // Date Filters
            DateTime from = DateTime.UtcNow;
            DateTime to = DateTime.UtcNow;
            switch (periodType)
            {
                case PeriodType.LastWeek:
                    from = DateTime.UtcNow.StartOfWeek(DayOfWeek.Monday).AddDays(-7);
                    to = DateTime.UtcNow.EndOfWeek(DayOfWeek.Monday).AddDays(-7);
                    break;
                case PeriodType.ThisWeek:
                    from = DateTime.UtcNow.StartOfWeek(DayOfWeek.Monday);
                    to = DateTime.UtcNow.EndOfWeek(DayOfWeek.Monday);
                    break;
                case PeriodType.ThisMonth:
                    from = DateTime.UtcNow.StartOfMonth();
                    to = DateTime.UtcNow.EndOfMonth();
                    break;
            }

            Query.Where(g => g.AttendanceDate >= from);
            Query.Where(g => g.AttendanceDate <= to);
            
            Query.Select(x => x.Id);
        }
    }
}
