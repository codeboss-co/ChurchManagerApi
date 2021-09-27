using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChurchManager.Application.Abstractions.Services
{
    public interface IServiceAsync<TEntity, TDto>
    {
        Task<IEnumerable<TDto>> ListAsync(CancellationToken ct = default);
        Task<TDto> GetByIdAsync(int id, CancellationToken ct = default);
        Task AddAsync(TDto tDto, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
        Task UpdateAsync(TDto entityTDto, CancellationToken ct = default);
    }
}
