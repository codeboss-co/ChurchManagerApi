using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.ViewModels;
using ChurchManager.Domain.Parameters;
using Convey.CQRS.Queries;

namespace ChurchManager.Application.Abstractions.Services
{
    public interface IPersonService
    {
        Task<PagedResult<PersonViewModel>> BrowseAsync(SearchTermQueryParameter query, CancellationToken ct = default);
    }
}
