using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.Groups.Specifications;
using ChurchManager.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class GroupDbRepository2 : GenericRepositoryBase<Group>, IGroupDbRepository2
    {
        public GroupDbRepository2(DbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<GroupMemberViewModel>> GroupMembersAsync(int groupId, RecordStatus status, CancellationToken ct = default)
        {
            var spec = new GroupMembersSpecification(groupId, status);
            var queryable = ApplySpecification(spec);

            var members = await queryable
                .SelectMany(x => x.Members)
                .Select(x => new GroupMemberViewModel
                {
                    PersonId = x.PersonId,
                    GroupId = x.GroupId,
                    GroupMemberId = x.Id,
                    FirstName = x.Person.FullName.FirstName,
                    MiddleName = x.Person.FullName.MiddleName,
                    LastName = x.Person.FullName.LastName,
                    Gender = x.Person.Gender,
                    PhotoUrl = x.Person.PhotoUrl,
                    GroupMemberRoleId = x.GroupRoleId,
                    GroupMemberRole = x.GroupRole.Name,
                    IsLeader = x.GroupRole.IsLeader,
                    FirstVisitDate = x.FirstVisitDate
                })
                .ToListAsync(ct);

            return members;
        }
    }
}
