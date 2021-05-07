using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ChurchManager.Application.Features.People.Queries;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Application.Features.People.Services
{

    public interface IPersonService
    {
        Task<PersonViewModel> PersonByUserLoginId(string userLoginId, CancellationToken ct = default);
    }

    public class PersonService : IPersonService
    {
        private readonly IPersonDbRepository _dbRepository;
        private readonly IMapper _mapper;

        public PersonService(IPersonDbRepository dbRepository, IMapper mapper)
        {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        public async Task<PersonViewModel> PersonByUserLoginId(string userLoginId, CancellationToken ct)
        {
            var vm = await _dbRepository.ProfileByUserLoginId(userLoginId)
                .ProjectTo<PersonViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);

            return vm;
        }
    }
}
