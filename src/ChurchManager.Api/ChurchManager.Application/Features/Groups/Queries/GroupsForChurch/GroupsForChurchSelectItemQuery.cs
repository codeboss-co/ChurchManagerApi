using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Application.Wrappers;
using ChurchManager.Core.Shared;
using ChurchManager.Domain.Features.Groups.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Application.Features.Groups.Queries.GroupsForChurch
{
    public record GroupsForChurchSelectItemQuery(int ChurchId) : IRequest<ApiResponse>;

    public class GroupsForChurchSelectItemHandler : IRequestHandler<GroupsForChurchSelectItemQuery, ApiResponse>
    {
        private readonly IGroupDbRepository _dbRepository;
        private readonly IMapper _mapper;

        public GroupsForChurchSelectItemHandler(IGroupDbRepository dbRepository, IMapper mapper)
        {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse> Handle(GroupsForChurchSelectItemQuery query, CancellationToken ct)
        {
            var vm = await _mapper
                .ProjectTo<SelectItemViewModel>(_dbRepository.Queryable())
                .ToListAsync(ct);

            return new ApiResponse(vm);
        }
    }
}
