using Ardalis.Specification;
using ChurchManager.Domain.Common;
using CodeBoss.Extensions;
using System;

namespace ChurchManager.Domain.Features.Groups.Specifications
{
    public class GroupMembersAttendanceAnalysisSpecification : Specification<GroupMemberAttendance>
    {
        public GroupMembersAttendanceAnalysisSpecification(int groupId, ReportPeriodType periodType)
        {
            Query.AsNoTracking();

            Query.Include("GroupMember.Person");

            // Group Filter
            Query.Where(g => g.GroupId == groupId);

            // Date Filters
            DateTime from = DateTime.UtcNow;
            DateTime to = DateTime.UtcNow.AddMonths(-3);
            switch (periodType)
            {
                case ReportPeriodType.Month:
                    from = DateTime.UtcNow.StartOfMonth();
                    to = DateTime.UtcNow.EndOfMonth();
                    break;
                case ReportPeriodType.SixMonths:
                    from = DateTime.UtcNow.StartOfMonth().AddMonths(-6);
                    to = DateTime.UtcNow.EndOfMonth().AddMonths(-6);
                    break;
                case ReportPeriodType.OneYear:
                    from = DateTime.UtcNow.StartOfMonth().AddMonths(-12);
                    to = DateTime.UtcNow.EndOfMonth().AddMonths(-12);
                    break;
            }

            Query.Where(g => g.AttendanceDate >= from);
            Query.Where(g => g.AttendanceDate <= to);

          
        }
    }
}
