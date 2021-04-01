using System;
using System.Linq.Expressions;
using ChurchManager.Persistence.Models.Groups;
using ChurchManager.Persistence.Shared;
using LinqKit;

namespace ChurchManager.Infrastructure.Persistence.Specifications
{
    public class BrowseGroupAttendanceSpecification : Specification<GroupAttendance>
    {
        // When the GroupId is 0 - it means we want to include all groups
        private const int AllGroupsId = 0;

        public BrowseGroupAttendanceSpecification(
            int groupTypeId,
            int? churchId,
            int? groupId,
            bool? withFeedback,
            DateTime? from, DateTime? to)
        {
            // Group Type Filter
            Expression<Func<GroupAttendance, bool>> groupTypeFilter = g => g.Group.GroupTypeId == groupTypeId;

            Criteria = groupTypeFilter;

            // Church Filter
            if (churchId.HasValue)
            {
                Expression<Func<GroupAttendance, bool>> churchFilter = g => g.Group.ChurchId.HasValue && g.Group.ChurchId.Value == churchId;
                Criteria = Criteria.And(churchFilter);
            }

            // Group Filter
            if(groupId.HasValue && groupId != AllGroupsId)
            {
                Expression<Func<GroupAttendance, bool>> groupFilter = g => g.Group.Id == groupId;
                Criteria = Criteria.And(groupFilter);
            }

            // Include attendance with feedback
            if(withFeedback.HasValue && withFeedback.Value)
            {
                Expression<Func<GroupAttendance, bool>> feedBackFilter = g => g.AttendanceReviewed;
                Criteria = Criteria.And(feedBackFilter);
            }

            // Date Filters
            if(from.HasValue)
            {
                Expression<Func<GroupAttendance, bool>> fromFilter = g => g.AttendanceDate >= from.Value;
                Criteria = Criteria.And(fromFilter);
            }
            if(to.HasValue)
            {
                Expression<Func<GroupAttendance, bool>> toFilter = g => g.AttendanceDate <= to.Value;
                Criteria = Criteria.And(toFilter);
            }

            IncludeStrings.Add("Group.GroupType");
        }
    }
}
