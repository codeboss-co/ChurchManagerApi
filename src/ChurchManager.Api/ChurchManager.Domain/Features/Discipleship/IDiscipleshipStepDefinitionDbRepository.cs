using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Core.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Persistence.Models.Discipleship;

namespace ChurchManager.Domain.Features.Discipleship
{
    public interface IDiscipleshipStepDefinitionDbRepository : IGenericRepositoryAsync<DiscipleshipStepDefinition>
    {
        Task<IEnumerable<DiscipleshipStepViewModel>> DiscipleshipStepInfoForPersonAsync(int personId, int discipleshipStepDefinitionId, CancellationToken ct = default);
    }
}
