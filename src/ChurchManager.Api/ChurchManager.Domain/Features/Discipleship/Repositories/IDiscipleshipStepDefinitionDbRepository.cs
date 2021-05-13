using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Domain.Features.Discipleship.Repositories
{
    public interface IDiscipleshipStepDefinitionDbRepository : IGenericDbRepository<DiscipleshipStepDefinition>
    {
        Task<IEnumerable<DiscipleshipStepViewModel>> DiscipleshipStepInfoForPersonAsync(int personId, int discipleshipStepDefinitionId, CancellationToken ct = default);
    }
}
