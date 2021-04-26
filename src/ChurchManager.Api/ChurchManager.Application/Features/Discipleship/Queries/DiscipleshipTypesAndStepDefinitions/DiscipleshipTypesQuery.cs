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
    public record DiscipleshipTypesQuery : IRequest<ApiResponse>
    {
    }

    public class DiscipleshipProgramsHandler : IRequestHandler<DiscipleshipTypesQuery, ApiResponse>
    {
        private readonly IGenericRepositoryAsync<DiscipleshipType> _dbRepository;
        private readonly IMapper _mapper;

        public DiscipleshipProgramsHandler(IGenericRepositoryAsync<DiscipleshipType> dbRepository, IMapper mapper)
        {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse> Handle(DiscipleshipTypesQuery query, CancellationToken ct)
        {
            var vm = await _mapper
                .ProjectTo<GeneralViewModel>(_dbRepository.Queryable().AsNoTracking())
                .ToListAsync(ct);

            return new ApiResponse(vm);
        }
    }
}
