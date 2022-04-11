using ChurchManager.Domain.Features.Groups;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Domain.Specifications
{
    public class GroupAttendanceSpecification : Specification<GroupAttendance>
    {
        public GroupAttendanceSpecification(int attendanceId)
        {
            Criteria = x => x.Id == attendanceId;

            Includes.Add(x => x.Group );
            IncludeStrings.Add("Attendees.GroupMember.Person");
        }
    }
}
