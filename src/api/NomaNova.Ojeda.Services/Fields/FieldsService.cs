using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using NomaNova.Ojeda.Core.Domain.Assets;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Core.Exceptions;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Dtos.Fields;
using NomaNova.Ojeda.Models.Shared;
using NomaNova.Ojeda.Services.Fields.Interfaces;
using NomaNova.Ojeda.Services.Fields.Validators;

namespace NomaNova.Ojeda.Services.Fields
{
    public class FieldsService : BaseService<Field>, IFieldsService
    {
        private readonly IRepository<FieldSet> _fieldSetsRepository;
        private readonly IRepository<Asset> _assetsRepository;
        
        public FieldsService(
            IMapper mapper,
            IRepository<Field> fieldsRepository,
            IRepository<FieldSet> fieldSetsRepository,
            IRepository<Asset> assetsRepository) : base(mapper, fieldsRepository)
        {
            _fieldSetsRepository = fieldSetsRepository;
            _assetsRepository = assetsRepository;
        }

        public async Task<FieldDto> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var field = await Repository.GetByIdAsync(id, cancellationToken);

            if (field == null)
            {
                throw new NotFoundException();
            }

            return Mapper.Map<FieldDto>(field);
        }

        public async Task<PaginatedListDto<FieldDto>> GetAsync(
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
                orderBy = nameof(Field.Name);
            }

            var paginatedFields = await Repository.GetAllPaginatedAsync(
                searchQuery, orderBy, orderAsc, excludedIds, pageNumber, pageSize, cancellationToken);

            var paginatedFieldsDto = Mapper.Map<PaginatedListDto<FieldDto>>(paginatedFields);
            paginatedFieldsDto.Items = paginatedFields.Select(f => Mapper.Map<FieldDto>(f)).ToList();

            return paginatedFieldsDto;
        }

        public async Task<FieldDto> CreateAsync(CreateFieldDto fieldDto, CancellationToken cancellationToken = default)
        {
            await ValidateAndThrowAsync(new CreateFieldDtoBusinessValidator(Repository), fieldDto, cancellationToken);

            var field = Mapper.Map<Field>(fieldDto);
            field.Id = Guid.NewGuid().ToString();

            field = await Repository.InsertAsync(field, cancellationToken);

            return Mapper.Map<FieldDto>(field);
        }

        public async Task<FieldDto> UpdateAsync(string id, UpdateFieldDto fieldDto,
            CancellationToken cancellationToken = default)
        {
            var field = await Repository.GetByIdAsync(id, cancellationToken);

            if (field == null)
            {
                throw new NotFoundException();
            }

            await ValidateAndThrowAsync(new UpdateFieldDtoBusinessValidator(Repository, id), fieldDto, cancellationToken);

            field = Mapper.Map(fieldDto, field);
            field.Id = id;
            field.Properties = Mapper.Map<FieldProperties>(fieldDto.Properties);
            
            field = await Repository.UpdateAsync(field, cancellationToken);

            return Mapper.Map<FieldDto>(field);
        }

        public async Task<DryRunDeleteFieldDto> DeleteAsync(string id, bool dryRun, CancellationToken cancellationToken = default)
        {
            var field = await Repository.GetByIdAsync(id, cancellationToken);

            if (field == null)
            {
                throw new NotFoundException();
            }

            var deleteFieldDto = new DryRunDeleteFieldDto();
            
            // Fetch impacted field sets
            var fieldSets = await _fieldSetsRepository.GetAllAsync(query =>
            {
                return query.Where(_ => _.FieldSetFields.Select(fsf => fsf.FieldId).Contains(id));
            }, cancellationToken);
            
            deleteFieldDto.FieldSets = fieldSets.Select(_ => Mapper.Map<NamedEntityDto>(_)).ToList();
            
            // Fetch impacted assets
            var assets = await _assetsRepository.GetAllAsync(
                query =>
                {
                    return query.Where(_ =>
                        _.FieldValues.Any(fv => fv.FieldId == id && fv.Value != null));
                },
                cancellationToken);
            
            deleteFieldDto.Assets = assets.Select(_ => Mapper.Map<NamedEntityDto>(_)).ToList();

            if (dryRun)
            {
                return deleteFieldDto;
            }

            await Repository.DeleteAsync(field, cancellationToken);
            return deleteFieldDto;
        }
    }
}