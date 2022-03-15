using AutoMapper;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Shared;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Features.Groups.Queries.GroupsForChurch
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
                .ProjectTo<SelectItemViewModel>(_dbRepository.Queryable().AsNoTracking().Where(x => x.ChurchId == query.ChurchId))
                .ToListAsync(ct);

            return new ApiResponse(vm.OrderBy(x => x.Name));
        }
    }
}