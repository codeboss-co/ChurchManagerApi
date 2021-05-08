﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Core.Shared;
using ChurchManager.Domain.Features.Discipleship;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class DiscipleshipDbRepository : GenericRepositoryAsync<DiscipleshipStepDefinition>, IDiscipleshipStepDefinitionDbRepository
    {
        public DiscipleshipDbRepository(IChurchManagerDbContext dbContext): base(dbContext)
        {
        }

        public async Task<IEnumerable<DiscipleshipStepViewModel>> DiscipleshipStepInfoForPersonAsync(int personId, int discipleshipStepDefinitionId, CancellationToken ct = default)
        {
            var vm = await Queryable()
                .AsNoTracking()
                .Include(x => x.DiscipleshipProgram)
                .Include(x => x.Steps)
                    .ThenInclude(x => x.Definition)
                .Where(x => x.Steps.Any(step => step.PersonId == personId) &&
                            x.Id == discipleshipStepDefinitionId)
                .SelectMany(x => x.Steps)
                .Select(x => new DiscipleshipStepViewModel
                {
                    CompletionDate = x.CompletionDate,
                    Status = x.Status,
                    IsComplete = x.IsComplete,
                    StepDefinition = new StepDefinitionViewModel { Name = x.Definition.Name, Description = x.Definition.Description }
                })
                .ToListAsync(ct);

            return vm;
        }
    }
}