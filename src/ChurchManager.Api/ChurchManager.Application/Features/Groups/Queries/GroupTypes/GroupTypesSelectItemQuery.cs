using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Application.Features.Groups.Queries.GroupTypes
{
    public record GroupTypesQuery : IRequest<ApiResponse>
    {
    }

    public class AllGroupTypesHandler : IRequestHandler<GroupTypesQuery, ApiResponse>
    {
        private readonly IGenericDbRepository<GroupType> _dbRepository;
        private readonly IMapper _mapper;

        public AllGroupTypesHandler(IGenericDbRepository<GroupType> dbRepository, IMapper mapper)
        {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse> Handle(GroupTypesQuery query, CancellationToken ct)
        {
            var vm = await _mapper
                .ProjectTo<SelectItemViewModel>(_dbRepository.Queryable())
                .ToListAsync(ct);

            return new ApiResponse(vm);
        }
    }
}
