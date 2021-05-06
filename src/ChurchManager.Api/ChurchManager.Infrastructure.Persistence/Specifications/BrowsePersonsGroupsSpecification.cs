using System;
using System.Linq;
using System.Linq.Expressions;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Persistence.Shared;
using LinqKit;

namespace ChurchManager.Infrastructure.Persistence.Specifications
{
    public class BrowsePersonsGroupsSpecification : Specification<Group>
    {
        public BrowsePersonsGroupsSpecification(int personId, string searchTerm)
        {
            // The groups this person is the leader of
            Expression<Func<Group, bool>> leadersGroupsFilter = g =>
                g.Members
                    .Any(m => m.PersonId == personId && m.GroupRole.IsLeader);

            Criteria = leadersGroupsFilter;

            // Filter the groups
            if(!string.IsNullOrEmpty(searchTerm))
            {
                Expression<Func<Group, bool>> groupFilter = g =>
                    g.Name.Contains(searchTerm) || g.Description.Contains(searchTerm);

                Criteria = leadersGroupsFilter.And(groupFilter);
            }

            Includes.Add(x => x.GroupType);
            IncludeStrings.Add("Members.GroupRole");
        }
    }
}
