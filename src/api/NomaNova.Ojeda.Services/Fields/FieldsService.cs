using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using NomaNova.Ojeda.Api.Exceptions;
using NomaNova.Ojeda.Core.Domain.Assets;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Dtos.Fields;
using NomaNova.Ojeda.Models.Shared;
using NomaNova.Ojeda.Services.Fields.Interfaces;
using NomaNova.Ojeda.Services.Fields.Validators;

namespace NomaNova.Ojeda.Services.Fields
{
    public class FieldsService : BaseService, IFieldsService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Field> _fieldsRepository;
        private readonly IRepository<FieldSet> _fieldSetRepository;
        private readonly IRepository<Asset> _assetsRepository;
        
        public FieldsService(
            IMapper mapper,
            IRepository<Field> fieldsRepository,
            IRepository<FieldSet> fieldSetsRepository,
            IRepository<Asset> assetsRepository)
        {
            _mapper = mapper;
            _fieldsRepository = fieldsRepository;
            _fieldSetRepository = fieldSetsRepository;
            _assetsRepository = assetsRepository;
        }

        public async Task<FieldDto> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var field = await _fieldsRepository.GetByIdAsync(id, cancellationToken);

            if (field == null)
            {
                throw new NotFoundException();
            }

            return _mapper.Map<FieldDto>(field);
        }

        public async Task<PaginatedListDto<FieldDto>> GetAsync(
            string searchQuery, 
            string orderBy, 
            bool orderAsc, 
            IList<string> excludedIds, 
            int pageNumber, 
            int pageSize, 
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = nameof(Field.Name);
            }

            var paginatedFields = await _fieldsRepository.GetAllPaginatedAsync(
                searchQuery, orderBy, orderAsc, excludedIds, pageNumber, pageSize, cancellationToken);

            var paginatedFieldsDto = _mapper.Map<PaginatedListDto<FieldDto>>(paginatedFields);
            paginatedFieldsDto.Items = paginatedFields.Select(f => _mapper.Map<FieldDto>(f)).ToList();

            return paginatedFieldsDto;
        }

        public async Task<FieldDto> CreateAsync(CreateFieldDto fieldDto, CancellationToken cancellationToken)
        {
            await ValidateAndThrowAsync(new CreateFieldDtoBusinessValidator(_fieldsRepository), fieldDto, cancellationToken);

            var field = _mapper.Map<Field>(fieldDto);
            field.Id = Guid.NewGuid().ToString();

            field = await _fieldsRepository.InsertAsync(field, cancellationToken);

            return _mapper.Map<FieldDto>(field);
        }

        public async Task<FieldDto> UpdateAsync(string id, UpdateFieldDto fieldDto,
            CancellationToken cancellationToken)
        {
            var field = await _fieldsRepository.GetByIdAsync(id, cancellationToken);

            if (field == null)
            {
                throw new NotFoundException();
            }

            await ValidateAndThrowAsync(new UpdateFieldDtoBusinessValidator(_fieldsRepository, id), fieldDto, cancellationToken);

            field = _mapper.Map(fieldDto, field);
            field.Id = id;
            field.Properties = _mapper.Map<FieldProperties>(fieldDto.Properties);
            
            field = await _fieldsRepository.UpdateAsync(field, cancellationToken);

            return _mapper.Map<FieldDto>(field);
        }

        public async Task<DryRunDeleteFieldDto> DeleteAsync(string id, bool dryRun, CancellationToken cancellationToken)
        {
            var field = await _fieldsRepository.GetByIdAsync(id, cancellationToken);

            if (field == null)
            {
                throw new NotFoundException();
            }

            var deleteFieldDto = new DryRunDeleteFieldDto();
            
            // Fetch impacted field sets
            var fieldSets = await _fieldSetRepository.GetAllAsync(query =>
            {
                return query.Where(_ => _.FieldSetFields.Select(fsf => fsf.FieldId).Contains(id));
            }, cancellationToken);
            
            deleteFieldDto.FieldSets = fieldSets.Select(_ => _mapper.Map<NamedEntityDto>(_)).ToList();
            
            // Fetch impacted assets
            var assets = await _assetsRepository.GetAllAsync(
                query =>
                {
                    return query.Where(_ =>
                        _.FieldValues.Any(fv => fv.FieldId == id && fv.Value != null));
                },
                cancellationToken);
            
            deleteFieldDto.Assets = assets.Select(_ => _mapper.Map<NamedEntityDto>(_)).ToList();

            if (dryRun)
            {
                return deleteFieldDto;
            }

            await _fieldsRepository.DeleteAsync(field, cancellationToken);
            return deleteFieldDto;
        }
    }
}