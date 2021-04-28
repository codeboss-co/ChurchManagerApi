using System.Linq;
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
                .Include(program => program.StepDefinitions)
                  .ThenInclude(definition => definition.Steps)
                .Where(program => program.StepDefinitions.Any(definition => definition.Steps.Any(step => step.PersonId == query.PersonId)))
                .Select(program => new DiscipleshipForPersonViewModel
                {
                    Program = new DiscipleshipProgramViewModel
                    {
                        Id = program.Id, 
                        Name = program.Name, 
                        Description = program.Description,
                        Category = program.Category,
                        Order = program.Order
                    },
                    Steps = program.StepDefinitions.SelectMany(definition => definition.Steps).Select(x => new DiscipleshipStepViewModel
                    {
                        CompletionDate = x.CompletionDate,
                        Status = x.Status,
                        IsComplete = x.IsComplete,
                        StepDefinition = new StepDefinitionViewModel {Order = x.Definition.Order, Id = x.Definition.Id, Description = x.Definition.Description, Name = x.Definition.Name},
                    })
                })
                .OrderBy(x => x.Program.Order)
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
