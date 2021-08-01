using System;
using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Core;
using NomaNova.Ojeda.Core.Helpers.Interfaces;
using NomaNova.Ojeda.Data.Context;

namespace NomaNova.Ojeda.Data.Repositories
{
    public class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ITimeKeeper _timeKeeper;
        private readonly DatabaseContext _context;
 
        public EntityRepository(
            ITimeKeeper timeKeeper, 
            DatabaseContext context)
        {
            _timeKeeper = timeKeeper;
            _context = context;
        }
        
        public async Task<TEntity> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _context.Set<TEntity>().FindAsync(new object[] {id}, cancellationToken);
        }

        public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));    
            }

            if (entity is ITimestampedEntity timestampedEntity)
            {
                var utcNow = _timeKeeper.UtcNow;
                
                timestampedEntity.CreatedAt = utcNow;
                timestampedEntity.UpdatedAt = utcNow;
            }

            await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));    
            }
            
            if (entity is ITimestampedEntity timestampedEntity)
            {
                timestampedEntity.UpdatedAt = _timeKeeper.UtcNow;
            }

            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));    
            }

            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}