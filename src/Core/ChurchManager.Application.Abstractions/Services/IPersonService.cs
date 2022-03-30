using ChurchManager.Application.ViewModels;
using ChurchManager.Domain.Features.People.Queries;
using Convey.CQRS.Queries;

namespace ChurchManager.Application.Abstractions.Services
{
    public interface IPersonService
    {
        Task<PagedResult<PersonViewModel>> BrowseAsync(PeopleAdvancedSearchQuery query, CancellationToken ct = default);
    }
}
