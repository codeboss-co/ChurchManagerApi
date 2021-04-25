using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Application.Wrappers;
using ChurchManager.Core.Shared.Parameters;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Model;
using Convey.CQRS.Queries;
using MediatR;

namespace ChurchManager.Application.Features.People.Queries.BrowsePeople
{
    public record BrowsePeopleQuery : SearchTermQueryParameter, IRequest<PagedResponse<PersonViewModel>>
    {
    }

    public class BrowsePeopleHandler : IRequestHandler<BrowsePeopleQuery, PagedResponse<PersonViewModel>>
    {
        private readonly IPersonDbRepository _dbRepository;
        private readonly IMapper _mapper;

        public BrowsePeopleHandler(IPersonDbRepository dbRepository, IMapper mapper)
        {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<PersonViewModel>> Handle(BrowsePeopleQuery query, CancellationToken ct)
        {
            var pagedResult = await _dbRepository.BrowsePeopleAsync(query, ct);

            try
            {
                var vm = _mapper.Map<PagedResult<PersonViewModel>>(pagedResult);

                return new PagedResponse<PersonViewModel>(vm);
            }
            catch(System.Exception ex)
            {

                throw;
            }
        }
    }
}
