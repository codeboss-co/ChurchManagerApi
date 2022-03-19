using ChurchManager.Application.ViewModels;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Infrastructure.Mapper;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Features.Churches.Queries.RetrieveChurches
{
    public class ChurchesQuery : IRequest<ApiResponse>
    {
    }

    public class AllChurchQueryHandler : IRequestHandler<ChurchesQuery, ApiResponse>
    {
        private readonly IGenericDbRepository<Church> _dbRepository;

        public AllChurchQueryHandler(IGenericDbRepository<Church> dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<ApiResponse> Handle(ChurchesQuery query, CancellationToken ct)
        {
            /*var vm = await _mapper
                .ProjectTo<ChurchViewModel>(_dbRepository.Queryable().OrderBy(x => x.Name))
                .ToListAsync(ct);*/

            var vm = await _dbRepository.Queryable().OrderBy(x => x.Name)
                .MapTo<Church, ChurchViewModel>()
                .ToListAsync(ct);

            return new ApiResponse(vm);
        }
    }
}