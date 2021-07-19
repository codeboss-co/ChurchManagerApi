using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Application.ViewModels;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Features.Discipleship;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Application.Features.Discipleship.Queries.DiscipleshipTypesAndStepDefinitions
{
    public record PeopleInDiscipleshipStepQuery(int DiscipleshipStepDefinitionId) : IRequest<ApiResponse>;

    public class PeopleInDiscipleshipStepHandler : IRequestHandler<PeopleInDiscipleshipStepQuery, ApiResponse>
    {
        private readonly IGenericDbRepository<DiscipleshipStep> _dbRepository;
        private readonly IMapper _mapper;

        public PeopleInDiscipleshipStepHandler(IGenericDbRepository<DiscipleshipStep> dbRepository, IMapper mapper)
        {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse> Handle(PeopleInDiscipleshipStepQuery query, CancellationToken ct)
        {
            var peopleInSteps = await _dbRepository
                .Queryable()
                .AsNoTracking()
                .Include(x => x.Person)
                .Include(x => x.Definition)
                .Where(x => x.DiscipleshipStepDefinitionId == query.DiscipleshipStepDefinitionId)
                .ToListAsync(ct);

            var vm = peopleInSteps.Select(step => new DiscipleshipStepViewModel
            {
                Person = new PersonViewModelBasic
                {
                    PersonId = step.PersonId,
                    FullName = step.Person.FullName,
                    Gender = step.Person.Gender,
                    AgeClassification = step.Person.AgeClassification,
                    PhotoUrl = step.Person.PhotoUrl,
                    BirthDate = step.Person.BirthDate,
                },
                CompletionDate = step.CompletionDate,
                Status = step.Status,
                IsComplete = step.IsComplete,
                StepDefinition = new StepDefinitionViewModel
                {
                    Order = step.Definition.Order,
                    Id = step.Definition.Id,
                    Description = step.Definition.Description,
                    Name = step.Definition.Name
                }
            })
            .OrderBy(step => step.StepDefinition.Order);

            return new ApiResponse(vm);
        }
    }
}
