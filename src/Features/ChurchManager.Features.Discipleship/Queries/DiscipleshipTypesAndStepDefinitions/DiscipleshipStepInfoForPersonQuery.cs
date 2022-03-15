using AutoMapper;
using ChurchManager.Domain.Features.Discipleship.Repositories;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Discipleship.Queries.DiscipleshipTypesAndStepDefinitions
{
    public record DiscipleshipStepInfoForPersonQuery : IRequest<ApiResponse>
    {
        public int? PersonId { get; set; }
        public int DiscipleshipStepDefinitionId { get; set; }
    }

    public class
        DiscipleshipStepInfoForPersonQueryHandler : IRequestHandler<DiscipleshipStepInfoForPersonQuery, ApiResponse>
    {
        private readonly IDiscipleshipStepDefinitionDbRepository _dbRepository;
        private readonly IMapper _mapper;

        public DiscipleshipStepInfoForPersonQueryHandler(IDiscipleshipStepDefinitionDbRepository dbRepository,
            IMapper mapper)
        {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse> Handle(DiscipleshipStepInfoForPersonQuery query, CancellationToken ct)
        {
            var vm =
                await _dbRepository.DiscipleshipStepInfoForPersonAsync(query.PersonId.Value,
                    query.DiscipleshipStepDefinitionId, ct);

            return new ApiResponse(vm);
        }
    }
}