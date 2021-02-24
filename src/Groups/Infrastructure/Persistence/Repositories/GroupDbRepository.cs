using System.Collections.Generic;
using System.Threading.Tasks;
using ChurchManager.Shared.Persistence;
using Domain.Repositories;
using Groups.Persistence.Models;
using Infrastructure.Persistence.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class GroupDbRepository : CrudDatabaseRepository<Group>, IGroupDbRepository
    {
        public GroupDbRepository(GroupsDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Group>> AllPersonsGroups(int personId)
        {
            var groups = await Queryable(new AllPersonsGroupsSpecification(personId)).ToListAsync();

            return groups;
        }
    }
}
