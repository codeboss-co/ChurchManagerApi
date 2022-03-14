using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain;
using ChurchManager.Domain.Features.Discipleship;
using ChurchManager.Domain.Features.Discipleship.Specifications;
using ChurchManager.Domain.Parameters;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using MediatR;

namespace ChurchManager.Application.Features.Discipleship.Queries.DiscipleshipTypesAndStepDefinitions
{
    public record BrowseDiscipleshipStepParticipantsQuery : SearchTermQueryParameter, IRequest<PagedResponse<DiscipleshipStepViewModel>>
    {
        public int DiscipleshipStepDefinitionId { get; set; }
        public string Status { get; set; }
        public DateTime? To { get; set; }
        public DateTime? From { get; set; }
    };

    public class PeopleInDiscipleshipStepHandler : IRequestHandler<BrowseDiscipleshipStepParticipantsQuery, PagedResponse<DiscipleshipStepViewModel>>
    {
        private readonly IGenericDbRepository<DiscipleshipStep> _dbRepository;
        private readonly IMapper _mapper;

        public PeopleInDiscipleshipStepHandler(IGenericDbRepository<DiscipleshipStep> dbRepository, IMapper mapper)
        {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<DiscipleshipStepViewModel>> Handle(BrowseDiscipleshipStepParticipantsQuery query, CancellationToken ct)
        {
            var spec = new BrowseDiscipleshipParticipants(query, query.DiscipleshipStepDefinitionId, query.Status, query.From, query.To);

            var pagedResult = await _dbRepository.BrowseAsync<DiscipleshipStepViewModel>(query, spec, ct);

            return new PagedResponse<DiscipleshipStepViewModel>(pagedResult);

            /*var peopleInSteps = await _dbRepository
                .Queryable()
                .AsNoTracking()
                .Include(x => x.Person)
                .Include(x => x.Definition)
                .Where(x => 
                    x.DiscipleshipStepDefinitionId == query.DiscipleshipStepDefinitionId &&
                    x.Status == query.Status )
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
                /*StepDefinition = new StepDefinitionViewModel
                {
                    Order = step.Definition.Order,
                    Id = step.Definition.Id,
                    Description = step.Definition.Description,
                    Name = step.Definition.Name,
                }#1#
            })
                .OrderBy(step => step.Status);

            return new ApiResponse(vm);*/
        }
    }
}
