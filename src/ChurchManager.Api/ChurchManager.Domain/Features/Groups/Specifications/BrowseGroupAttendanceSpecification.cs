using System;
using Ardalis.Specification;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Shared;
using Convey.CQRS.Queries;

namespace ChurchManager.Domain.Features.Groups.Specifications
{
    public class BrowseGroupAttendanceSpecification : Specification<GroupAttendance, GroupAttendanceViewModel>
    {
        // When the GroupId is 0 - it means we want to include all groups
        private const int AllGroupsId = 0;

        public BrowseGroupAttendanceSpecification(
            IPagedQuery paging,
            int groupTypeId,
            int? churchId,
            int? groupId,
            bool withFeedback,
            DateTime? from, DateTime? to)
        {
            Query.AsNoTracking();
            Query.Include("Group.GroupType");

            // Group Type Filter
            Query.Where(g => g.Group.GroupTypeId == groupTypeId);

            // Church Filter
            if(churchId.HasValue)
            {
                Query.Where(g => g.Group.ChurchId.HasValue && g.Group.ChurchId.Value == churchId);
            }

            // Group Filter
            if (groupId.HasValue && groupId != AllGroupsId)
            {
                Query.Where(g => g.Group.Id == groupId);
            }

            // Include attendance with feedback
            if (withFeedback)
            {
                Query.Where(g => g.AttendanceReview != null && g.AttendanceReview.IsReviewed == true);
            }
            else
            {
                Query.Where(g => g.AttendanceReview == null || g.AttendanceReview.IsReviewed == false);
            }

            // Date Filters
            if(from.HasValue)
            {
                Query.Where(g => g.AttendanceDate >= from.Value);
            }
            if(to.HasValue)
            {
                Query.Where(g => g.AttendanceDate <= to.Value);
            }

            Query.OrderBy(x => x.AttendanceDate);

            Query
                .Skip(paging.CalculateSkip())
                .Take(paging.CalculateTake());

            Query.Select(x => new GroupAttendanceViewModel
            {
                Id = x.Id,
                GroupName = x.Group.Name,
                AttendanceDate = x.AttendanceDate,
                DidNotOccur = x.DidNotOccur,
                AttendanceCount = x.AttendanceCount,
                FirstTimerCount = x.FirstTimerCount,
                NewConvertCount = x.NewConvertCount,
                ReceivedHolySpiritCount = x.ReceivedHolySpiritCount,
                Notes = x.Notes,
                PhotoUrls = x.PhotoUrls
            });
        }  
    }
}
