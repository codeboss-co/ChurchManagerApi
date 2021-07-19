using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Features.Discipleship;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using MediatR;

namespace ChurchManager.Application.Features.Discipleship.Queries.DiscipleshipTypesAndStepDefinitions
{
    public record PeopleInDiscipleshipStepQuery(int DiscipleshipStepDefinitionId) : IRequest<ApiResponse>;

    public class PeopleInDiscipleshipStepHandler : IRequestHandler<PeopleInDiscipleshipStepQuery, ApiResponse>
    {
        private readonly IGenericDbRepository<DiscipleshipStep> _dbRepository;
        private readonly IMapper _mapper;

        public PeopleInDiscipleshipStepHandler(IGenericDbRepository<DiscipleshipStep> dbRepository, IMapper mapper)
        {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        public Task<ApiResponse> Handle(PeopleInDiscipleshipStepQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
