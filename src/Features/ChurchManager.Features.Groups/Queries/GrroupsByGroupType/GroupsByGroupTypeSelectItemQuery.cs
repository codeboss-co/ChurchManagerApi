using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Application.Features.Groups.Queries.GrroupsByGroupType
{
    public record GroupsByGroupTypeSelectItemQuery(int GroupTypeId) : IRequest<ApiResponse>;

    public class GroupsByGroupTypeSelectItemHandler : IRequestHandler<GroupsByGroupTypeSelectItemQuery, ApiResponse>
    {
        private readonly IGroupDbRepository _dbRepository;
        private readonly IMapper _mapper;

        public GroupsByGroupTypeSelectItemHandler(IGroupDbRepository dbRepository, IMapper mapper)
        {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse> Handle(GroupsByGroupTypeSelectItemQuery query, CancellationToken ct)
        {
            var vm = await _mapper
                .ProjectTo<SelectItemViewModel>(
                    _dbRepository
                        .Queryable()
                        .AsNoTracking()
                        .Where(x => x.GroupTypeId == query.GroupTypeId)
                        .OrderBy(x => x.Name)
                    )
                .ToListAsync(ct);

            return new ApiResponse(vm);
        }
    }
}
