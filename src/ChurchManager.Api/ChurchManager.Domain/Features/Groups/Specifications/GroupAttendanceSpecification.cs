using Ardalis.Specification;

namespace ChurchManager.Domain.Features.Groups.Specifications
{
    public class GroupAttendanceSpecification : Specification<GroupAttendance>, ISingleResultSpecification
    {
        public GroupAttendanceSpecification(int attendanceId)
        {
            Query.AsNoTracking();
            Query.Where(x => x.Id == attendanceId);
            
            Query.Include(x => x.Group);
            Query.Include("Attendees.GroupMember.Person");
        }  
    }
}
