using System.Collections.Generic;
using System.Threading.Tasks;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Infrastructure.Abstractions
{
    public interface IDataShapeHelper<in T>
    {
        IEnumerable<DomainEntity> ShapeData(IEnumerable<T> entities, string fieldsString);
        Task<IEnumerable<DomainEntity>> ShapeDataAsync(IEnumerable<T> entities, string fieldsString);
        DomainEntity ShapeData(T entity, string fieldsString);
    }
}