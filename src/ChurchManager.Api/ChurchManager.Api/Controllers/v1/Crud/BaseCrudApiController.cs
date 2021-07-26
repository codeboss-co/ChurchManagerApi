using System;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Codeboss.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Api.Controllers.v1.Crud
{
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public abstract class BaseCrudApiController<TEntity> : ControllerBase where TEntity : class, IAggregateRoot<int>
    {
        private IGenericDbRepository<TEntity> _repository;
        protected IGenericDbRepository<TEntity> Repository => _repository ??= HttpContext.RequestServices.GetRequiredService<IGenericDbRepository<TEntity>>();

        [HttpGet]
        public virtual async Task<IActionResult> GetList(CancellationToken ct)
        {
            var entities = await Repository.ListAsync(ct);
            return Ok(entities);
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Get(Guid id, CancellationToken ct)
        {
            var entity = await Repository.GetByIdAsync(id, ct);
            return Ok(entity);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post(TEntity entity, CancellationToken ct)
        {
            var created = await Repository.AddAsync(entity, ct);
            return Created(HttpContext.Request.Path, created.Id);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put(Guid id, TEntity updated, CancellationToken ct)
        {
            var entity = await Repository.GetByIdAsync(id, ct);

            if(entity is not null)
            {
                Repository.DbContext.Entry(entity).CurrentValues.SetValues(updated);
            }

            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var entity = await Repository.GetByIdAsync(id, ct);

            if (entity is not null)
            {
                await Repository.DeleteAsync(entity, ct);
            }

            return Ok(entity);
        }
    }
}
