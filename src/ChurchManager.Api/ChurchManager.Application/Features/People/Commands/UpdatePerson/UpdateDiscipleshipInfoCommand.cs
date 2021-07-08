using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Features.Discipleship;
using ChurchManager.Domain.Features.Discipleship.Repositories;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Application.Features.People.Commands.UpdatePerson
{
    public record UpdateDiscipleshipInfoCommand : IRequest
    {
        public int PersonId { get; set; }
        public bool? ReceivedHolySpirit { get; set; }
        public bool? IsBaptised { get; set; }
        public DateTime? BaptismDate { get; set; }     
        public bool? IsCompleteFoundationSchool { get; set; }
        public DateTime? FoundationSchoolDate { get; set; }
    }

    public class UpdateDiscipleshipInfoHandler : IRequestHandler<UpdateDiscipleshipInfoCommand>
    {
        private readonly IPersonDbRepository _dbRepository;
        private readonly IGenericDbRepository<DiscipleshipStep> _stepsDbRepository;

        public UpdateDiscipleshipInfoHandler(
            IPersonDbRepository dbRepository,
            IGenericDbRepository<DiscipleshipStep> stepsDbRepository)
        {
            _dbRepository = dbRepository;
            _stepsDbRepository = stepsDbRepository;
        }

        public async Task<Unit> Handle(UpdateDiscipleshipInfoCommand command, CancellationToken ct)
        {
            var person = await _dbRepository.GetByIdAsync(command.PersonId, ct);

            if (person is not null)
            {
                // Baptism
                #region Update person profile

                var baptism = person.BaptismStatus;
                if(baptism is null)
                {
                    baptism = new Baptism { IsBaptised = command.IsBaptised, BaptismDate = command.BaptismDate };
                }
                else
                {
                    baptism.IsBaptised = command.IsBaptised;
                    baptism.BaptismDate = command.BaptismDate;
                }

                person.BaptismStatus = baptism;
                person.ReceivedHolySpirit = command.ReceivedHolySpirit;

                await _dbRepository.SaveChangesAsync(ct); 

                #endregion

                #region  Update person baptism discipleship step

                if(baptism.IsBaptised.HasValue && baptism.IsBaptised.Value)
                {
                    int baptismStepDefinitionId = 2;
                    var baptismStep = await _stepsDbRepository
                        .Queryable()
                        .Where(x =>
                                    x.PersonId == command.PersonId &&
                                    x.DiscipleshipStepDefinitionId == baptismStepDefinitionId)
                        .FirstOrDefaultAsync(ct);

                    if(baptismStep is null)
                    {
                        await _stepsDbRepository.AddAsync(new DiscipleshipStep
                        {
                            PersonId = command.PersonId,
                            DiscipleshipStepDefinitionId = baptismStepDefinitionId,
                            CompletionDate = command.BaptismDate,
                            Status = "Completed"
                        }, ct);
                    }
                    else
                    {
                        baptismStep.Status = "Completed";
                        await _stepsDbRepository.SaveChangesAsync(ct);
                    }
                }

                #endregion

                #region  Update person foundation school discipleship step

                if(command.IsCompleteFoundationSchool.HasValue && command.IsCompleteFoundationSchool.Value)
                {
                    int foundationSchoolStepDefinitionId = 1;
                    var foundationSchoolStep = await _stepsDbRepository
                        .Queryable()
                        .Where(x =>
                            x.PersonId == command.PersonId &&
                            x.DiscipleshipStepDefinitionId == foundationSchoolStepDefinitionId)
                        .FirstOrDefaultAsync(ct);

                    if(foundationSchoolStep is null)
                    {
                        await _stepsDbRepository.AddAsync(new DiscipleshipStep
                        {
                            PersonId = command.PersonId,
                            DiscipleshipStepDefinitionId = foundationSchoolStepDefinitionId,
                            CompletionDate = command.FoundationSchoolDate,
                            Status = "Completed"
                        }, ct);
                    }
                    else
                    {
                        foundationSchoolStep.Status = "Completed";
                        await _stepsDbRepository.SaveChangesAsync(ct);
                    }
                }

                #endregion
            }

            return new Unit();
        }
    }
}