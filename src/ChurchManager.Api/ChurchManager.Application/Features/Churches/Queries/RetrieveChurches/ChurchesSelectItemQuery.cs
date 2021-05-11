using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Application.ViewModels;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Application.Features.Churches.Queries.RetrieveChurches
{
    public class ChurchesQuery : IRequest<ApiResponse>
    {
    }

    public class AllChurchQueryHandler : IRequestHandler<ChurchesQuery, ApiResponse>
    {
        private readonly IGenericDbRepository<Church> _dbRepository;
        private readonly IMapper _mapper;

        public AllChurchQueryHandler(IGenericDbRepository<Church> dbRepository, IMapper mapper)
        {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse> Handle(ChurchesQuery query, CancellationToken ct)
        {
            var vm =  await _mapper
                .ProjectTo<ChurchViewModel>(_dbRepository.Queryable())
                .ToListAsync(ct);

            return new ApiResponse(vm);
        }
    }
}
