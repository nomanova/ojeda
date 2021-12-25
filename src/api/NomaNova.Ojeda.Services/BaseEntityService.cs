using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using NomaNova.Ojeda.Core;
using NomaNova.Ojeda.Core.Exceptions;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Services
{
    public abstract class BaseEntityService<T> : BaseService where T : BaseEntity, INamedEntity
    {
        protected readonly IMapper Mapper;
        protected readonly IRepository<T> Repository;
        
        protected BaseEntityService(
            IMapper mapper,
            IRepository<T> repository)
        {
            Mapper = mapper;
            Repository = repository;
        }

        protected async Task<TS> GetByIdAsync<TS>(string id, CancellationToken cancellationToken = default)
        {
            var entity = await Repository.GetByIdAsync(id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException();
            }

            return Mapper.Map<TS>(entity);
        }
        
        protected async Task<PaginatedListDto<TS>> GetAsync<TS>(
            string searchQuery,
            string orderBy,
            bool orderAsc,
            IList<string> excludedIds,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = nameof(INamedEntity.Name);
            }
        
            var paginatedEntities = await Repository.GetAllPaginatedAsync(
                searchQuery, orderBy, orderAsc, excludedIds, pageNumber, pageSize, cancellationToken);
        
            var paginatedEntitiesDto = Mapper.Map<PaginatedListDto<TS>>(paginatedEntities);
            paginatedEntitiesDto.Items = paginatedEntities.Select(f => Mapper.Map<TS>(f)).ToList();

            return paginatedEntitiesDto;
        }
    }
}