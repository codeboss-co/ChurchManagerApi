using ChurchManager.Domain;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Discipleship;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Features.Discipleship.Queries.DiscipleshipTypesAndStepDefinitions
{
    public record DiscipleshipProgramsQuery : IRequest<ApiResponse>
    {
        public int? DiscipleshipProgramId { get; set; }
    };

    public class DiscipleshipProgramHandler : IRequestHandler<DiscipleshipProgramsQuery, ApiResponse>
    {
        private readonly IGenericDbRepository<DiscipleshipProgram> _dbRepository;

        public DiscipleshipProgramHandler(IGenericDbRepository<DiscipleshipProgram> dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<ApiResponse> Handle(DiscipleshipProgramsQuery query, CancellationToken ct)
        {
            #region  Single Program

            if(query.DiscipleshipProgramId.HasValue)
            {
                var program = await _dbRepository
                    .Queryable()
                    .AsNoTracking()
                    .Include(x => x.StepDefinitions)
                        .ThenInclude(y => y.Steps)
                    .Where(x =>
                        x.RecordStatus == RecordStatus.Active &&
                        x.Id == query.DiscipleshipProgramId.Value)
                    .Select(x => new
                    {
                        Program = new DiscipleshipProgramViewModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Description = x.Description,
                            Category = x.Category,
                            Order = x.Order,
                            IconCssClass = "heroicons_outline:template",
                            CreatedDate = x.CreatedDate,
                            ModifiedDate = x.ModifiedDate
                        },
                        StepDefinitions = x.StepDefinitions.Select(stepDef => new StepDefinitionViewModel
                        {
                            Id = stepDef.Id,
                            AllowMultiple = stepDef.AllowMultiple,
                            Name = stepDef.Name,
                            Description = stepDef.Description,
                            Order = stepDef.Order,
                            IconCssClass = stepDef.IconCssClass,
                            Statistics = new StepDefinitionStatistics
                            {
                                InProgress = stepDef.Steps.Count(step => step.Status == DomainConstants.Discipleship.StepStatus.InProgress),
                                Completed = stepDef.Steps.Count(step => step.Status == DomainConstants.Discipleship.StepStatus.Completed)
                            }
                        }),
                        /*Statistics = new {
                            Started = x.StepDefinitions.SelectMany(x => x.Steps).Select(y => y.CompletionDate).AsEnumerable().Count(completeDate => completeDate.HasValue == false),
                            Completed = x.StepDefinitions.SelectMany(x => x.Steps).Select(y => y.CompletionDate).AsEnumerable().Count(completeDate => completeDate.HasValue),
                        }*/
                    })
                    .FirstOrDefaultAsync(ct);

                return new ApiResponse(program);
            }
            #endregion

            #region  Multiple Programs

            var programs = await _dbRepository
                    .Queryable()
                    .AsNoTracking()
                    .Include(x => x.StepDefinitions)
                    .Where(x => x.RecordStatus == RecordStatus.Active)
                    .Select(x => new
                    {
                        Program = new DiscipleshipProgramViewModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Description = x.Description,
                            Category = x.Category,
                            Order = x.Order,
                            IconCssClass = "heroicons_outline:template",
                            CreatedDate = x.CreatedDate,
                            ModifiedDate = x.ModifiedDate
                        },
                        StepDefinitions = x.StepDefinitions.Select(stepDef => new StepDefinitionViewModel
                        {
                            Id = stepDef.Id,
                            AllowMultiple = stepDef.AllowMultiple,
                            Name = stepDef.Name,
                            Description = stepDef.Description,
                            Order = stepDef.Order,
                            IconCssClass = stepDef.IconCssClass
                        })
                    })
                    .ToListAsync(ct);

            // Sort the step definitions
            programs.ForEach(x => x.StepDefinitions.OrderBy(x => x.Order));
            return new ApiResponse(programs); 

            #endregion
        }
    }
}