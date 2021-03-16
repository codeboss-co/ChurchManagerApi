using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Persistence.Models.Groups;

namespace ChurchManager.Domain.Features.Groups.Repositories
{
    public interface IGroupAttendanceDbRepository : IGenericRepositoryAsync<GroupAttendance>
    {
    }
}
