using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
                .ToListAsync(ct);

            // Sort the step definitions
            programs.ForEach(x => x.StepDefinitions.OrderBy(x => x.Order));
            return new ApiResponse(programs);
        }
    }
}