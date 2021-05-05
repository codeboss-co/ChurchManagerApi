using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Application.Wrappers;
using ChurchManager.Core.Shared;
using ChurchManager.Domain.Model.Discipleship;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Application.Features.Discipleship.Queries.DiscipleshipTypesAndStepDefinitions
{
    public record DiscipleshipDefinitionStepsQuery(int DiscipleshipTypeId) : IRequest<ApiResponse>
    {
    }

    public class DiscipleshipDefinitionStepsHandler : IRequestHandler<DiscipleshipDefinitionStepsQuery, ApiResponse>
    {
        private readonly IGenericRepositoryAsync<DiscipleshipStepDefinition> _dbRepository;
        private readonly IMapper _mapper;

        public DiscipleshipDefinitionStepsHandler(IGenericRepositoryAsync<DiscipleshipStepDefinition> dbRepository, IMapper mapper)
        {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse> Handle(DiscipleshipDefinitionStepsQuery query, CancellationToken ct)
        {
            var vm = await _mapper
                .ProjectTo<GeneralViewModel>(
                    _dbRepository
                                .Queryable().Where(x => x.DiscipleshipProgramId == query.DiscipleshipTypeId)
                                .AsNoTracking())
                .ToListAsync(ct);

            return new ApiResponse(vm);
        }
    }
}
