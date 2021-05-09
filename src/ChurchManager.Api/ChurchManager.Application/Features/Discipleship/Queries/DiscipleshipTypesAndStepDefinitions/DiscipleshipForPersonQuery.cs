using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Application.Abstractions;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Features.Discipleship;
using ChurchManager.Infrastructure.Abstractions.Persistence;
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
        private readonly IGenericRepositoryAsync<DiscipleshipStep> _dbRepository;
        private readonly IMapper _mapper;

        public DiscipleshipForPersonHandler(IGenericRepositoryAsync<DiscipleshipStep> dbRepository, IMapper mapper)
        {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse> Handle(DiscipleshipForPersonQuery query, CancellationToken ct)
        {
            var vm = await _dbRepository.Queryable("Definition.DiscipleshipProgram")
                .Where(program => program.PersonId == query.PersonId)
                .Select(step => new
                {
                    Program = new DiscipleshipProgramViewModel
                    {
                        Id = step.Definition.DiscipleshipProgram.Id,
                        Name = step.Definition.DiscipleshipProgram.Name,
                        Description = step.Definition.DiscipleshipProgram.Description,
                        Category = step.Definition.DiscipleshipProgram.Category,
                        Order = step.Definition.DiscipleshipProgram.Order
                    },
                    Step = new DiscipleshipStepViewModel
                    {
                        CompletionDate = step.CompletionDate,
                        Status = step.Status,
                        IsComplete = step.IsComplete,
                        StepDefinition = new StepDefinitionViewModel
                        {
                            Order = step.Definition.Order, Id = step.Definition.Id,
                            Description = step.Definition.Description, Name = step.Definition.Name
                        },
                    }
                })
                .OrderBy(x => x.Step.StepDefinition.Order)
                .ToListAsync(ct);
                
            // This is done in memory
            var grouping = vm.GroupBy(x => x.Program.Id);

            var programs = grouping.Select(x =>
            {
                var program = x.FirstOrDefault()?.Program;
                var steps = x.Select(definition => definition?.Step);

                if (program is null) return new DiscipleshipForPersonViewModel();

                return new DiscipleshipForPersonViewModel
                {
                    Program = new DiscipleshipProgramViewModel
                    {
                        Id = program.Id,
                        Name = program.Name,
                        Description = program.Description,
                        Category = program.Category,
                        Order = program.Order
                    },
                    Steps = steps.Select(x =>
                        new DiscipleshipStepViewModel
                        {
                            CompletionDate = x.CompletionDate,
                            Status = x.Status,
                            IsComplete = x.IsComplete,
                            StepDefinition = new StepDefinitionViewModel
                            {
                                Order = x.StepDefinition.Order, Id = x.StepDefinition.Id,
                                Description = x.StepDefinition.Description, Name = x.StepDefinition.Name
                            },
                        })
                };
            });

            return new ApiResponse(programs);
        }
    }
}
