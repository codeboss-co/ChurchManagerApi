using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Model.Discipleship;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Domain.Features.Discipleship
{
    public interface IDiscipleshipStepDefinitionDbRepository : IGenericRepositoryAsync<DiscipleshipStepDefinition>
    {
        Task<IEnumerable<DiscipleshipStepViewModel>> DiscipleshipStepInfoForPersonAsync(int personId, int discipleshipStepDefinitionId, CancellationToken ct = default);
    }
}
