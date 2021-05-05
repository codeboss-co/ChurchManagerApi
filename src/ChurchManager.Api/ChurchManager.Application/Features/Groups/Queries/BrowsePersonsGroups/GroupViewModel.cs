using System.Collections.Generic;
using ChurchManager.Application.Features.People.Queries;
using ChurchManager.Persistence.Models.Groups;

// https://anthonygiretti.com/2020/06/17/introducing-c-9-records/#:~:text=Constructors%20and%20deconstructors%20are%20allowed,That%27s%20good%20!&text=As%20you%20may%20noticed%2C%20this,is%20determined%20by%20their%20position.
namespace ChurchManager.Application.Features.Groups.Queries.BrowsePersonsGroups
{
    public record GroupViewModel : RecordStatusViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentGroupId { get; set; }
        public GroupTypeViewModel GroupType { get; set; }
        public ICollection<GroupMemberViewModel> Members { get; set; }
    }

    public record GroupTypeViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool TakesAttendance { get; set; }

        public GroupTypeViewModel(GroupType entity) 
            => (Name, Description, TakesAttendance) = (entity.Name, entity.Description, entity.TakesAttendance);
    }

    public record GroupMemberViewModel : RecordStatusViewModel
    {
        public string GroupMemberRole { get; set; }
        public bool IsLeader { get; set; }
        public PersonViewModel Person { get; set; }

        public GroupMemberViewModel(GroupMember entity)
        {
            GroupMemberRole = entity.GroupRole.Name;
            IsLeader = entity.GroupRole.IsLeader;
        }
    }

    public record GroupMemberRoleViewModel : RecordStatusViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsLeader { get; set; }
    }

    public record RecordStatusViewModel
    {
        public string RecordStatus { get; set; }
    }
}
