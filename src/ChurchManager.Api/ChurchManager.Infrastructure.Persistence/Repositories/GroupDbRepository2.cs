using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.Groups.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class GroupDbRepository2 : GenericRepositoryBase<Group>, IGroupDbRepository2
    {
        public GroupDbRepository2(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
