using System;
using Ardalis.Specification;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.Groups.Specifications
{
    public class AttendanceReportGridSpecification : Specification<GroupAttendance, GroupAttendanceViewModel>
    { 
        // When the GroupId is 0 - it means we want to include all groups
        private const int AllGroupsId = 0;

        public AttendanceReportGridSpecification(int groupTypeId, DateTime from, DateTime to, int? groupId = null)
        {
            Query.AsNoTracking();
            Query.Include("Group.GroupType");
            Query.Include("Group.Church");

            // Group Type Filter
            Query.Where(g => g.Group.GroupTypeId == groupTypeId);
            
            // Group Filter
            if(groupId.HasValue && groupId != AllGroupsId)
            {
                Query.Where(g => g.Group.Id == groupId);
            }

            // Date Filters
            Query.Where(g => g.AttendanceDate >= from);
            Query.Where(g => g.AttendanceDate <= to);

            // Only show meetings that occurred
            Query.Where(g => g.DidNotOccur == null || g.DidNotOccur.Value == false);

            Query.Select(x => new GroupAttendanceViewModel
            {
                ChurchName = x.Group.Church.Name,
                GroupName = x.Group.Name,
                AttendanceDate = x.AttendanceDate,
                AttendanceCount = x.AttendanceCount,
                FirstTimerCount = x.FirstTimerCount,
                NewConvertCount = x.NewConvertCount,
                ReceivedHolySpiritCount = x.ReceivedHolySpiritCount
            });
        }
    }
}
