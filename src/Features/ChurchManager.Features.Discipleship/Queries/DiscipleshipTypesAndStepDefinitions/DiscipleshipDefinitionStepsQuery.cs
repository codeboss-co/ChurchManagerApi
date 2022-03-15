using AutoMapper;
using ChurchManager.Domain.Features.Discipleship;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Features.Discipleship.Queries.DiscipleshipTypesAndStepDefinitions
{
    public record DiscipleshipDefinitionStepsQuery(int DiscipleshipTypeId) : IRequest<ApiResponse>
    {
    }

    public class DiscipleshipDefinitionStepsHandler : IRequestHandler<DiscipleshipDefinitionStepsQuery, ApiResponse>
    {
        private readonly IGenericDbRepository<DiscipleshipStepDefinition> _dbRepository;
        private readonly IMapper _mapper;

        public DiscipleshipDefinitionStepsHandler(IGenericDbRepository<DiscipleshipStepDefinition> dbRepository,
            IMapper mapper)
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