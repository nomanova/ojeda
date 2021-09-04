using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Core;

namespace NomaNova.Ojeda.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> GetByIdAsync(
            string id, CancellationToken cancellationToken = default);

        Task<List<TEntity>> GetAllAsync(
            Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null,
            CancellationToken cancellationToken = default);
        
        Task<PaginatedList<TEntity>> GetAllPaginatedAsync(
            string searchQuery,
            string orderBy, bool orderAsc,
            int pageNumber, int pageSize,
            CancellationToken cancellationToken);

        Task<TEntity> InsertAsync(
            TEntity entity, CancellationToken cancellationToken = default);

        Task<TEntity> UpdateAsync(
            TEntity entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(
            TEntity entity, CancellationToken cancellationToken = default);
    }
}