using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.ViewModels;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Discipleship;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Application.Features.Discipleship.Queries.DiscipleshipTypesAndStepDefinitions
{
    public record DiscipleshipProgramsQuery : IRequest<ApiResponse>;

    public class DiscipleshipProgramHandler : IRequestHandler<DiscipleshipProgramsQuery, ApiResponse>
    {
        private readonly IGenericDbRepository<DiscipleshipProgram> _dbRepository;

        public DiscipleshipProgramHandler(IGenericDbRepository<DiscipleshipProgram> dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<ApiResponse> Handle(DiscipleshipProgramsQuery request, CancellationToken ct)
        {
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
        }
    }
}