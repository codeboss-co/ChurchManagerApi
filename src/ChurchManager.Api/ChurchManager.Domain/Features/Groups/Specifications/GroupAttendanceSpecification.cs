using System;
using Ardalis.Specification;
using ChurchManager.Domain.Common;
using CodeBoss.Extensions;

namespace ChurchManager.Domain.Features.Groups.Specifications
{
    public class GroupAttendanceSpecification : Specification<GroupAttendance>, ISingleResultSpecification
    {
        public GroupAttendanceSpecification(int attendanceId)
        {
            Query.AsNoTracking();
            Query.Include("Attendees.GroupMember.Person");
            Query.Include(x => x.Group);

            Query.Where(x => x.Id == attendanceId);
        }  
    }

    public class GroupAttendancesByGroupSpecification : Specification<GroupAttendance>
    {
        public GroupAttendancesByGroupSpecification(int groupId, PeriodType periodType)
        {
            Query.AsNoTracking();
            Query.Include("Attendees.GroupMember.Person");

            // Group Filter
            Query.Where(x => x.GroupId == groupId);

            // Date Filters
            DateTime from = DateTime.UtcNow;
            DateTime to = DateTime.UtcNow.AddMonths(-3);
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

            Query.Where(g => g.DidNotOccur == null || (g.DidNotOccur.HasValue && !g.DidNotOccur.Value));
        }
    }
}
