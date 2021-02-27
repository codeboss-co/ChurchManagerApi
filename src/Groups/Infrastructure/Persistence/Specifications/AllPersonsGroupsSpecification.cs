using System.Linq;
using ChurchManager.Shared.Persistence;
using Groups.Persistence.Models;

namespace Infrastructure.Persistence.Specifications
{
    public class AllPersonsGroupsSpecification : Specification<Group>
    {
        public AllPersonsGroupsSpecification(int personId)
        {
            Criteria = x =>
                x.Members
                    .Any(x => x.PersonId == personId &&
                              x.GroupMemberRole.IsLeader);

            Includes.Add(x => x.GroupType);
            Includes.Add(x => x.Members);
            IncludeStrings.Add(("Members.GroupMemberRole"));
            IncludeStrings.Add(("Members.Person"));
        }
    }

    public class BrowsePersonsGroupsSpecification : Specification<Group>
    {
        public BrowsePersonsGroupsSpecification(int personId, string searchTerm)
        {
            if (!string.IsNullOrEmpty(searchTerm))
            {
                Criteria = x =>
                    x.Members
                        .Any(x => x.PersonId == personId &&
                                  x.GroupMemberRole.IsLeader) &&
                    (x.Name.Contains(searchTerm) || x.Description.Contains(searchTerm));
            }
            else
            {
                Criteria = x =>
                    x.Members
                        .Any(x => x.PersonId == personId &&
                                  x.GroupMemberRole.IsLeader);
            }

            Includes.Add(x => x.GroupType);
            Includes.Add(x => x.Members);
            IncludeStrings.Add(("Members.GroupMemberRole"));
            IncludeStrings.Add(("Members.Person"));
        }
    }
}
