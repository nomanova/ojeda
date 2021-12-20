using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using NomaNova.Ojeda.Core;
using NomaNova.Ojeda.Core.Exceptions;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Shared;
using NomaNova.Ojeda.Utils.Extensions;
using ValidationException = NomaNova.Ojeda.Core.Exceptions.ValidationException;

namespace NomaNova.Ojeda.Services
{
    public abstract class BaseService<T> where T : BaseEntity, INamedEntity
    {
        protected readonly IMapper Mapper;
        protected readonly IRepository<T> Repository;
        
        protected BaseService(
            IMapper mapper,
            IRepository<T> repository)
        {
            Mapper = mapper;
            Repository = repository;
        }

        protected static async Task ValidateAndThrowAsync<TR>(AbstractValidator<TR> validator, TR instance,
            CancellationToken cancellationToken)
        {
            var validationErrorList = await ValidateAsync(validator, instance, cancellationToken);

            if (validationErrorList.HasElements())
            {
                throw new ValidationException(validationErrorList);
            }
        }

        protected static async Task<Dictionary<string, List<string>>> ValidateAsync<TR>(AbstractValidator<TR> validator,
            TR instance,
            CancellationToken cancellationToken)
        {
            var validationErrorList = new Dictionary<string, List<string>>();

            if (instance == null)
            {
                validationErrorList.Add(typeof(TR).Name, new List<string> { "Invalid entity." });
                return validationErrorList;
            }

            var result = await validator.ValidateAsync(instance, cancellationToken);

            if (!result.IsValid)
            {
                validationErrorList = result.Errors
                    .GroupBy(
                        e => e.PropertyName,
                        e => e.ErrorMessage,
                        (field, messages) => new { Field = field, Messages = messages.ToList() })
                    .ToDictionary(t => t.Field, t => t.Messages);
            }

            return validationErrorList;
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