using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Application.Features.People.Commands.DeletePerson
{
    /// <summary>
    /// This will hard delete person and also remove from all groups 
    /// </summary>
    public record DeletePersonCommand(int PersonId) : IRequest;

    public class DeletePersonHandler : IRequestHandler<DeletePersonCommand>
    {
        private readonly IPersonDbRepository _dbRepository;
        private readonly IGenericDbRepository<Family> _familyRepository;

        public DeletePersonHandler(
            IPersonDbRepository dbRepository,
            IGenericDbRepository<Family> familyRepository)
        {
            _dbRepository = dbRepository;
            _familyRepository = familyRepository;
        }

        public async Task<Unit> Handle(DeletePersonCommand command, CancellationToken ct)
        {
            var person = await _dbRepository.GetByIdAsync(command.PersonId, ct);

            if (person is not null)
            {
                await _dbRepository.DeleteAsync(person, ct);

                // Delete family if this is the last family member
                if (person.FamilyId.HasValue)
                {
                    // Check for family members
                    var isLastFamilyMember = !await _dbRepository
                        .Queryable()
                        .AsNoTracking()
                        .AnyAsync(x => x.FamilyId == person.FamilyId, ct);

                    if (isLastFamilyMember)
                    {
                        var family = await _familyRepository.GetByIdAsync(person.FamilyId, ct);
                        if (family is not null)
                        {
                            await _familyRepository.DeleteAsync(family, ct);
                        }
                    }
                }
            }

            return Unit.Value;
        }
    }
}
