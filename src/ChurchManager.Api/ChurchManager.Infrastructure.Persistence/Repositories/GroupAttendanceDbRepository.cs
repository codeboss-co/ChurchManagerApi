using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Infrastructure.Persistence.Contexts;
using ChurchManager.Persistence.Models.Groups;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class GroupAttendanceDbRepository : GenericRepositoryAsync<GroupAttendance>, IGroupAttendanceDbRepository
    {
        public GroupAttendanceDbRepository(ChurchManagerDbContext dbContext) : base(dbContext)
        {
        }
    }
}
