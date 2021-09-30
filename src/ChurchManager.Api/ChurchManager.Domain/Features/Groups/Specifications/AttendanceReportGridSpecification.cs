using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.Specification;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.Groups.Specifications
{
    public class AttendanceReportGridSpecification : Specification<GroupAttendance, GroupAttendanceViewModel>
    { 
        // When the GroupId is 0 - it means we want to include all groups
        private const int AllGroupsId = 0;

        public AttendanceReportGridSpecification(int groupTypeId, IList<int> groupId, DateTime from, DateTime to)
        {
            Query.AsNoTracking();
            Query.Include("Group.GroupType");
            Query.Include("Group.Church");

            // Group Type Filter
            Query.Where(g => g.Group.GroupTypeId == groupTypeId);
            
            // Group Filter
            if(groupId != null && groupId.Any() && !groupId.Contains(AllGroupsId))
            {
                Query.Where(g => groupId.Contains(g.Group.Id));
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
                ReceivedHolySpiritCount = x.ReceivedHolySpiritCount,
                Offering = x.Offering == null
                    ? new MoneyViewModel{Amount = 0, Currency = "ZAR"}
                    : new MoneyViewModel { Amount = x.Offering.Amount, Currency = x.Offering.Currency }
            });
        }
    }
}
