using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.People.Queries.GetFamily
{
    public record GetFamilyQuery(int FamilyId) : IRequest<ApiResponse>;

    public class GetFamilyHandler : IRequestHandler<GetFamilyQuery, ApiResponse>
    {
        private readonly IGenericDbRepository<Family> _dbRepository;

        public GetFamilyHandler(IGenericDbRepository<Family> dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<ApiResponse> Handle(GetFamilyQuery query, CancellationToken ct)
        {
            var family = await _dbRepository.GetByIdAsync(query.FamilyId, ct) ?? throw new ArgumentNullException(nameof(query.FamilyId));

            // Strip Family from name
            var index = family.Name.IndexOf("Family", StringComparison.InvariantCultureIgnoreCase);
            var vm = new FamilyViewModel
            {
                Id = family.Id,
                Name = index > -1 ? family.Name.Remove(index - 1, 7) : family.Name
            };

            return new ApiResponse(vm);
        }
    }
}
 
