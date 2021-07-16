using System;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using CodeBoss.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Application.Features.People.Commands.DeletePerson
{
    /// <summary>
    /// This will hard delete person and also remove from all groups 
    /// </summary>
    public record DeletePersonCommand(int PersonId) : IRequest<ApiResponse>;

    public class DeletePersonHandler : IRequestHandler<DeletePersonCommand, ApiResponse>
    {
        private readonly IPersonDbRepository _dbRepository;
        private readonly IGenericDbRepository<Family> _familyRepository;
        private readonly ILogger<DeletePersonHandler> _logger;

        public DeletePersonHandler(
            IPersonDbRepository dbRepository,
            IGenericDbRepository<Family> familyRepository,
            ILogger<DeletePersonHandler> logger)
        {
            _dbRepository = dbRepository;
            _familyRepository = familyRepository;
            _logger = logger;
        }

        public async Task<ApiResponse> Handle(DeletePersonCommand command, CancellationToken ct)
        {
            try
            {
                var person = await _dbRepository
                       .Queryable()
                       .Include(x => x.PhoneNumbers)
                       .Include(x => x.Notes)
                       .FirstOrDefaultAsync(x => x.Id == command.PersonId, ct);

                if(person is not null)
                {
                    await _dbRepository.DeleteAsync(person, ct);

                    // Delete family if this is the last family member
                    if(person.FamilyId.HasValue)
                    {
                        // Check for family members
                        var isLastFamilyMember = !await _dbRepository
                            .Queryable()
                            .AsNoTracking()
                            .AnyAsync(x => x.FamilyId == person.FamilyId, ct);

                        if(isLastFamilyMember)
                        {
                            var family = await _familyRepository.GetByIdAsync(person.FamilyId, ct);
                            if(family is not null)
                            {
                                await _familyRepository.DeleteAsync(family, ct);
                            }
                        }
                    }

                    // Cleanup
                    if (!person.UserLoginId.IsNullOrEmpty())
                    {
                        // TODO: delete user logins 
                    }
                }
            }
            catch(Exception ex)
            {
                var message = $"Problem deleting person with Id: {command.PersonId}";
                _logger.LogError(ex, message);
                return new ApiResponse(message);
            }

            return new ApiResponse(true);
        }
    }
}
