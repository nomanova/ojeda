using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NomaNova.Ojeda.Core;
using NomaNova.Ojeda.Data.Context;
using NomaNova.Ojeda.Data.Options;
using NomaNova.Ojeda.Utils.Extensions;
using NomaNova.Ojeda.Utils.Services.Interfaces;

namespace NomaNova.Ojeda.Data.Repositories
{
    public class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ITimeKeeper _timeKeeper;
        private readonly DatabaseContext _context;
        private readonly DatabaseOptions _options;
 
        public EntityRepository(
            ITimeKeeper timeKeeper, 
            DatabaseContext context,
            IOptions<DatabaseOptions> options)
        {
            _timeKeeper = timeKeeper;
            _context = context;
            _options = options.Value;
        }
        
        public async Task<TEntity> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<TEntity>().FindAsync(new object[] {id}, cancellationToken);
        }

        public async Task<TEntity> GetByIdAsync(
            string id, 
            Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, 
            CancellationToken cancellationToken = default)
        {
            var queryable = _context.Set<TEntity>().Where(e => e.Id.Equals(id));
            
            queryable = func != null ? func(queryable) : queryable;
            
            return await queryable.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<TEntity>> GetAllAsync(
            Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, CancellationToken cancellationToken = default)
        {
            var queryable = _context.Set<TEntity>().AsQueryable();
            
            queryable = func != null ? func(queryable) : queryable;

            return await queryable.ToListAsync(cancellationToken);
        }

        public async Task<PaginatedList<TEntity>> GetAllPaginatedAsync(
            string searchQuery,
            string orderBy, 
            bool orderAsc,
            IList<string> excludedIds,
            int pageNumber, 
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var queryable = _context.Set<TEntity>().AsQueryable();

            if (excludedIds.HasElements())
            {
                queryable = queryable.Where(e => !excludedIds.Contains(e.Id));
            }

            queryable = queryable
                .ExecuteSearchQueryFilter(searchQuery, _options.Type)
                .ExecuteOrderBy(orderBy, orderAsc);
            
            return await PaginatedList<TEntity>.CreateAsync(queryable, pageNumber, pageSize, cancellationToken);
        }

        public async Task<PaginatedList<TEntity>> GetAllPaginatedAsync(
            string searchQuery,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> func,
            string orderBy, bool orderAsc,
            int pageNumber, int pageSize,
            CancellationToken cancellationToken = default)
        {
            var queryable = _context.Set<TEntity>().AsQueryable();
            
            queryable = queryable
                .ExecuteSearchQueryFilter(searchQuery, _options.Type)
                .ExecuteOrderBy(orderBy, orderAsc);
            
            queryable = func != null ? func(queryable) : queryable;
            
            return await PaginatedList<TEntity>.CreateAsync(queryable, pageNumber, pageSize, cancellationToken);
        }

        public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
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

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
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

        public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));    
            }

            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
        
        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities)); 
            }

            _context.Set<TEntity>().RemoveRange(entities);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}