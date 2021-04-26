﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Application.Wrappers;
using ChurchManager.Core.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Persistence.Models.Discipleship;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Application.Features.Discipleship.Queries.DiscipleshipTypesAndStepDefinitions
{
    public record DiscipleshipForPersonQuery : IRequest<ApiResponse>
    {
        public int? PersonId { get; set; }
    }

    public class DiscipleshipForPersonHandler : IRequestHandler<DiscipleshipForPersonQuery, ApiResponse>
    {
        private readonly IGenericRepositoryAsync<DiscipleshipProgram> _dbRepository;
        private readonly IMapper _mapper;

        public DiscipleshipForPersonHandler(IGenericRepositoryAsync<DiscipleshipProgram> dbRepository, IMapper mapper)
        {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse> Handle(DiscipleshipForPersonQuery query, CancellationToken ct)
        {
            var vm = await _dbRepository.Queryable()
                .Include(x => x.DiscipleshipSteps)
                  .ThenInclude(x => x.Definition)
                    .ThenInclude(x => x.DiscipleshipType)
                .Where(x => x.DiscipleshipSteps.Any(s => s.PersonId == query.PersonId))
                .Select(x => new DiscipleshipForPersonViewModel
                {
                    Program = new GeneralViewModel {Name = x.Name, Id = x.Id, Description = x.Description},
                    Steps = x.DiscipleshipSteps.Select(x => new DiscipleshipStepsViewModel
                    {
                        CompletionDate = x.CompletionDate,
                        Status = x.Status,
                        StepDefinition = new StepDefinitionViewModel {Order = x.Definition.Order, Id = x.Definition.Id, Description = x.Definition.Description, Name = x.Definition.Name},
                        DiscipleshipType = new GeneralViewModel
                        {
                            Id = x.Definition.DiscipleshipType.Id,
                            Description = x.Definition.DiscipleshipType.Description,
                            Name = x.Definition.DiscipleshipType.Name
                        }
                    })
                })
                .ToListAsync(ct);

            // Ordering
            vm.ForEach(x =>
            {
                x.Steps = x.Steps.OrderBy(s => s.StepDefinition.Order);
            });

            return new ApiResponse(vm);
        }
    }
}