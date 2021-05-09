using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Features.Discipleship;
using MediatR;

namespace ChurchManager.Application.Features.Discipleship.Queries.DiscipleshipTypesAndStepDefinitions
{
    public record DiscipleshipStepInfoForPersonQuery : IRequest<ApiResponse>
    {
        public int? PersonId { get; set; }
        public int DiscipleshipStepDefinitionId { get; set; }
    }

    public class DiscipleshipStepInfoForPersonQueryHandler : IRequestHandler<DiscipleshipStepInfoForPersonQuery, ApiResponse>
    {
        private readonly IDiscipleshipStepDefinitionDbRepository _dbRepository;
        private readonly IMapper _mapper;

        public DiscipleshipStepInfoForPersonQueryHandler(IDiscipleshipStepDefinitionDbRepository dbRepository, IMapper mapper)
        {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse> Handle(DiscipleshipStepInfoForPersonQuery query, CancellationToken ct)
        {
            var vm = 
                await _dbRepository.DiscipleshipStepInfoForPersonAsync(query.PersonId.Value, query.DiscipleshipStepDefinitionId, ct);

            return new ApiResponse(vm);
        }
    }
}
