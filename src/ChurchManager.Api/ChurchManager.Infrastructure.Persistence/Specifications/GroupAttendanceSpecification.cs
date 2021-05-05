using ChurchManager.Domain.Model.Groups;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Infrastructure.Persistence.Specifications
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
