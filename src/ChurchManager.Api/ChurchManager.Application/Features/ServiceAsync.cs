using AutoMapper;
using ChurchManager.Application.Abstractions.Models;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Codeboss.Types;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChurchManager.Application.Features
{
    public class ServiceAsync<TEntity, TDto> : IServiceAsync<TEntity, TDto>
        where TDto : EntityDto where TEntity : class, IAggregateRoot<int>
    {
        protected readonly IGenericDbRepository<TEntity> Repository;
        private readonly IMapper _mapper;

        public ServiceAsync(IGenericDbRepository<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<IEnumerable<TDto>> ListAsync(CancellationToken ct = default)
        {
            var entities = await Repository.ListAsync(ct);
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }

        public virtual async Task<TDto> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await Repository.GetByIdAsync(id, ct);
            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task AddAsync(TDto tDto, CancellationToken ct = default)
        {
            var entity = _mapper.Map<TEntity>(tDto);
            await Repository.AddAsync(entity, ct);
        }

        public virtual async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await Repository.GetByIdAsync(id, ct) ?? throw new ArgumentNullException("id", $"Entity with id: {id} not found");
            await Repository.DeleteAsync(entity, ct);
        }

        public virtual async Task UpdateAsync(TDto entityTDto, CancellationToken ct = default)
        {
            var entity = _mapper.Map<TEntity>(entityTDto);
            await Repository.UpdateAsync(entity, ct);
        }
    }
}
