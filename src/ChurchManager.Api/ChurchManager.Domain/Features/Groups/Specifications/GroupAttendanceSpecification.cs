using System;
using System.Linq;
using Ardalis.Specification;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Shared;
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

    public class GroupAttendancesByGroupSpecification : Specification<GroupAttendance, GroupAttendanceViewModel>
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

            Query.Select(entity => new GroupAttendanceViewModel
            {
                Id = entity.Id,
                // TODO: ChurchName = ???
                GroupName = entity.Group.Name,
                AttendanceDate = entity.AttendanceDate,
                AttendanceRate = entity.AttendanceRate,
                Attendees = entity.Attendees.Select(entityAttendee => new GroupMemberAttendanceViewModel
                {
                    AttendanceDate = entityAttendee.AttendanceDate,
                    GroupMemberId = entityAttendee.GroupMemberId,
                    DidAttend = entityAttendee.DidAttend,
                    GroupMember = new GroupMemberViewModel
                    {
                        PersonId = entityAttendee.GroupMember.PersonId,
                        FirstName = entityAttendee.GroupMember.Person.FullName.FirstName,
                        MiddleName = entityAttendee.GroupMember.Person.FullName.MiddleName,
                        LastName = entityAttendee.GroupMember.Person.FullName.LastName,
                        PhotoUrl = entityAttendee.GroupMember.Person.PhotoUrl,
                        // TODO: MiddleName = ???
                        // TODO: LastName = ???
                        // TODO: Gender = ???
                        // TODO: PhotoUrl = ???
                        // TODO: GroupMemberRoleId = ???
                        // TODO: GroupMemberRole = ???
                        // TODO: IsLeader = ???
                        RecordStatus = entityAttendee.GroupMember.RecordStatus
                    }
                })
            });
        }
    }
}
