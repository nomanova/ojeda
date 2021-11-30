using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Api.Exceptions;
using NomaNova.Ojeda.Core.Domain.Assets;
using NomaNova.Ojeda.Core.Domain.AssetTypes;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Dtos.Assets;
using NomaNova.Ojeda.Models.Dtos.Assets.Base;
using NomaNova.Ojeda.Models.Dtos.AssetTypes;
using NomaNova.Ojeda.Models.Shared;
using NomaNova.Ojeda.Models.Shared.Validation;
using NomaNova.Ojeda.Services.Assets.Interfaces;
using ValidationException = NomaNova.Ojeda.Core.Exceptions.ValidationException;

namespace NomaNova.Ojeda.Services.Assets
{
    public class AssetsService : BaseService, IAssetsService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Asset> _assetsRepository;
        private readonly IRepository<AssetType> _assetTypesRepository;
        private readonly IRepository<FieldSet> _fieldSetsRepository;
        private readonly IRepository<Field> _fieldsRepository;
        private readonly IFieldDataConverter _fieldDataConverter;

        public AssetsService(
            IMapper mapper,
            IRepository<Asset> assetsRepository,
            IRepository<AssetType> assetTypesRepository,
            IRepository<FieldSet> fieldSetsRepository,
            IRepository<Field> fieldsRepository,
            IFieldDataConverter fieldDataConverter)
        {
            _mapper = mapper;
            _assetsRepository = assetsRepository;
            _assetTypesRepository = assetTypesRepository;
            _fieldSetsRepository = fieldSetsRepository;
            _fieldsRepository = fieldsRepository;
            _fieldDataConverter = fieldDataConverter;
        }

        public async Task<AssetDto> GetByAssetTypeAsync(string assetTypeId, CancellationToken cancellationToken)
        {
            var assetType = await _assetTypesRepository.GetByIdAsync(assetTypeId, query =>
            {
                return query
                    .Include(s => s.AssetTypeFieldSets);
            }, cancellationToken);
            
            if (assetType == null)
            {
                throw new NotFoundException();
            }
            
            var fieldSets = await GetFieldSetsAsync(assetType, cancellationToken);
            var fields = await GetFieldsAsync(fieldSets, cancellationToken);
            
            return AssetTypeToAssetDtoAsync(assetType, fieldSets, fields, 
                (_, _, fieldProperties) => _fieldDataConverter.FromBytes(null, fieldProperties));
        }

        public async Task<AssetDto> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var asset = await _assetsRepository.GetByIdAsync(id, query =>
            {
                return query
                    .Include(s => s.AssetType)
                    .ThenInclude(f => f.AssetTypeFieldSets)
                    .Include(a => a.FieldValues);
            }, cancellationToken);
            
            if (asset == null)
            {
                throw new NotFoundException();
            }

            var assetType = asset.AssetType;
            var fieldValues = asset.FieldValues;
            
            var fieldSets = await GetFieldSetsAsync(assetType, cancellationToken);
            var fields = await GetFieldsAsync(fieldSets, cancellationToken);

            var assetDto = AssetTypeToAssetDtoAsync(assetType, fieldSets, fields, (fieldSetId, fieldId, fieldData) =>
            {
                var fieldValue = fieldValues.FirstOrDefault(_ => _.FieldSetId.Equals(fieldSetId) && _.FieldId.Equals(fieldId));
                var byteValue = fieldValue?.Value;

                return _fieldDataConverter.FromBytes(byteValue, fieldData);
            });
            
            assetDto.Id = id;
            assetDto.Name = asset.Name;
            assetDto.UpdatedAt = asset.UpdatedAt;

            return assetDto;
        }

        public async Task<PaginatedListDto<AssetSummaryDto>> GetAsync(
            string searchQuery, 
            string orderBy, 
            bool orderAsc,
            int pageNumber, 
            int pageSize, 
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = nameof(Field.Name);
            }

            var paginatedAssets = await _assetsRepository.GetAllPaginatedAsync(
                searchQuery, query =>
                {
                    return query.Include(_ => _.AssetType);
                }, orderBy, orderAsc, pageNumber, pageSize, cancellationToken);

            var paginatedAssetsDto = _mapper.Map<PaginatedListDto<AssetSummaryDto>>(paginatedAssets);
            paginatedAssetsDto.Items = paginatedAssets.Select(f => _mapper.Map<AssetSummaryDto>(f)).ToList();

            return paginatedAssetsDto;
        }

        public async Task<AssetDto> CreateAsync(CreateAssetDto assetDto, CancellationToken cancellationToken)
        {
            // Validate
            var (dbFieldSets, dbFields) = await ValidateUpsertAsync(assetDto, cancellationToken);

            // Store
            var dtoFieldSets = assetDto.FieldSets;

            var asset = _mapper.Map<Asset>(assetDto);
            
            asset.Id = Guid.NewGuid().ToString();
            asset.FieldValues = new List<FieldValue>();
            
            foreach (var dbFieldSet in dbFieldSets)
            {
                var dbFieldIds = dbFieldSet.FieldSetFields.Select(_ => _.FieldId).ToList();
                var dbFieldSetFields = dbFields.Where(_ => dbFieldIds.Contains(_.Id)).ToList();

                var dtoFieldSet = dtoFieldSets.First(_ => _.Id.Equals(dbFieldSet.Id));
                
                foreach (var dbField in dbFieldSetFields)
                {
                    var dtoField = dtoFieldSet.Fields.First(_ => _.Id.Equals(dbField.Id));
                    
                    var fieldValue = new FieldValue
                    {
                        Id = Guid.NewGuid().ToString(),
                        AssetId = asset.Id,
                        FieldSetId = dbFieldSet.Id,
                        FieldId = dbField.Id,
                        Value = _fieldDataConverter.ToBytes(dtoField.Data, dbField.Properties)
                    };
                    
                    asset.FieldValues.Add(fieldValue);
                }
            }

            await _assetsRepository.InsertAsync(asset, cancellationToken);

            // Return
            return await GetByIdAsync(asset.Id, cancellationToken);
        }

        public async Task<AssetDto> UpdateAsync(string id, UpdateAssetDto assetDto,
            CancellationToken cancellationToken)
        {
            // Fetch
            var asset = await _assetsRepository.GetByIdAsync(id, query =>
            {
                return query.Include(s => s.FieldValues);
            }, cancellationToken);

            if (asset == null)
            {
                throw new NotFoundException();
            }
            
            // Validate
            var (dbFieldSets, dbFields) = await ValidateUpsertAsync(assetDto, cancellationToken);
            
            // Store
            var dtoFieldSets = assetDto.FieldSets;
            var fieldValues = asset.FieldValues;

            asset = _mapper.Map(assetDto, asset);
            asset.Id = id;

            var updatedFieldValues = new List<FieldValue>();

            foreach (var dbFieldSet in dbFieldSets)
            {
                var dbFieldIds = dbFieldSet.FieldSetFields.Select(_ => _.FieldId).ToList();
                var dbFieldSetFields = dbFields.Where(_ => dbFieldIds.Contains(_.Id)).ToList();

                var dtoFieldSet = dtoFieldSets.First(_ => _.Id.Equals(dbFieldSet.Id));

                foreach (var dbField in dbFieldSetFields)
                {
                    var dtoField = dtoFieldSet.Fields.First(_ => _.Id.Equals(dbField.Id));

                    var existingFieldValue = fieldValues.FirstOrDefault(_ => _.AssetId.Equals(id) &&
                                                                             _.FieldSetId.Equals(dbFieldSet.Id) &&
                                                                             _.FieldId.Equals(dbField.Id));

                    if (existingFieldValue != null)
                    {
                        existingFieldValue.Value = _fieldDataConverter.ToBytes(dtoField.Data, dbField.Properties);
                        updatedFieldValues.Add(existingFieldValue);
                    }
                    else
                    {
                        // Field added after initial creation
                        var newFieldValue = new FieldValue
                        {
                            Id = Guid.NewGuid().ToString(),
                            AssetId = asset.Id,
                            FieldSetId = dbFieldSet.Id,
                            FieldId = dbField.Id,
                            Value = _fieldDataConverter.ToBytes(dtoField.Data, dbField.Properties)
                        };
                        
                        updatedFieldValues.Add(newFieldValue);
                    }
                }
            }

            asset.FieldValues = updatedFieldValues;
            
            await _assetsRepository.UpdateAsync(asset, cancellationToken);
            
            // Return
            return await GetByIdAsync(asset.Id, cancellationToken);
        }
        
        public async Task DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var asset = await _assetsRepository.GetByIdAsync(id, cancellationToken);

            if (asset == null)
            {
                throw new NotFoundException();
            }

            await _assetsRepository.DeleteAsync(asset, cancellationToken);
        }

        private AssetDto AssetTypeToAssetDtoAsync(
            AssetType assetType, 
            IReadOnlyCollection<FieldSet> fieldSets,
            IReadOnlyCollection<Field> fields,
            Func<string, string, FieldProperties, FieldDataDto> fieldValueResolver = null)
        {
            var assetDto = new AssetDto
            {
                Id = null,
                AssetType = _mapper.Map<AssetTypeSummaryDto>(assetType)
            };

            foreach (var assetTypeFieldSet in assetType.AssetTypeFieldSets)
            {
                var fieldSetId = assetTypeFieldSet.FieldSetId;
                var fieldSet = fieldSets.FirstOrDefault(_ => _.Id.Equals(fieldSetId));
                
                if (fieldSet == null)
                {
                    throw new Exception($"Field set {fieldSetId} does not belong to asset type {assetTypeFieldSet.AssetTypeId}");
                }
                
                var fieldSetDto = _mapper.Map<AssetFieldSetDto>(fieldSet);
                fieldSetDto.Order = assetTypeFieldSet.Order;

                foreach (var fieldSetField in fieldSet.FieldSetFields)
                {
                    var fieldId = fieldSetField.FieldId;
                    var field = fields.FirstOrDefault(_ => _.Id.Equals(fieldId));
                    
                    if (field == null)
                    {
                        throw new Exception($"Field {fieldId} does not belong to field set {fieldSetField.FieldSetId}");
                    }

                    var fieldDto = _mapper.Map<AssetFieldDto>(field);
                    fieldDto.Order = fieldSetField.Order;
                    fieldDto.IsRequired = fieldSetField.IsRequired;
                    
                    // Value
                    if (fieldValueResolver != null)
                    {
                        fieldDto.Data = fieldValueResolver(fieldSetId, fieldId, field.Properties);
                    }

                    fieldSetDto.Fields.Add(fieldDto);
                }

                fieldSetDto.Fields = fieldSetDto.Fields
                    .OrderBy(_ => _.Order)
                    .ThenBy(_ => _.Name)
                    .ToList();
                
                assetDto.FieldSets.Add(fieldSetDto);
            }

            assetDto.FieldSets = assetDto.FieldSets
                .OrderBy(_ => _.Order)
                .ThenBy(_ => _.Name)
                .ToList();
            
            return assetDto;
        }

        private async Task<List<FieldSet>> GetFieldSetsAsync(AssetType assetType, CancellationToken cancellationToken)
        {
            var fieldSetIds = assetType.AssetTypeFieldSets
                .Select(_ => _.FieldSetId)
                .Distinct()
                .ToList();

            var fieldSets = await _fieldSetsRepository.GetAllAsync(query =>
            {
                return query
                    .Where(_ => fieldSetIds.Contains(_.Id))
                    .Include(_ => _.FieldSetFields);
            }, cancellationToken);

            return fieldSets;
        }

        private async Task<List<Field>> GetFieldsAsync(IEnumerable<FieldSet> fieldSets, CancellationToken cancellationToken)
        {
            var fieldIds = fieldSets
                .SelectMany(_ => _.FieldSetFields)
                .Select(_ => _.FieldId)
                .Distinct()
                .ToList();

            var fields = await _fieldsRepository.GetAllAsync(query =>
            {
                return query.Where(_ => fieldIds.Contains(_.Id));
            }, cancellationToken);

            return fields;
        }

        private async Task<(List<FieldSet>, List<Field>)> ValidateUpsertAsync<T, TS>(UpsertAssetDto <T, TS>  assetDto, CancellationToken cancellationToken)
            where T : UpsertAssetFieldSetDto<TS> where TS : UpsertAssetFieldDto
        {
            // Step 1: field validation
            var validationErrors = await ValidateAsync(new NamedFieldValidator(), assetDto, cancellationToken);
            
            // Step 2: ensure the asset type exists
            var assetType = await _assetTypesRepository.GetByIdAsync(assetDto.AssetTypeId, query =>
            {
                return query
                    .Include(s => s.AssetTypeFieldSets);
            }, cancellationToken);

            if (assetType == null)
            {
                validationErrors.Add(nameof(CreateAssetDto.AssetTypeId), new List<string>{"The asset type does not exist."});
                throw new ValidationException(validationErrors);
            }

            // Step 3: ensure all expected field sets are present
            var dbFieldSets = await GetFieldSetsAsync(assetType, cancellationToken);
            var dtoFieldSets = assetDto.FieldSets ?? new List<T>();
            
            var dbFieldSetIds = dbFieldSets.Select(_ => _.Id).ToList();
            var dtoFieldSetIds = dtoFieldSets.Select(_ => _.Id).ToList();
            
            var missingFieldSetIds = dbFieldSetIds.Except(dtoFieldSetIds).ToList();
            
            if (missingFieldSetIds.Any())
            {
                var messages = missingFieldSetIds.Select(_ => $"Field set with id {_} is missing.").ToList();
                validationErrors.Add(nameof(CreateAssetDto.FieldSets), messages);
                
                throw new ValidationException(validationErrors);
            }

            // Step 4: ensure all expected fields are present
            foreach (var dbFieldSet in dbFieldSets)
            {
                var dbFieldIds = dbFieldSet.FieldSetFields.Select(_ => _.FieldId).ToList();

                var dtoFieldSet = dtoFieldSets.First(_ => _.Id.Equals(dbFieldSet.Id));

                var dtoFieldIds = dtoFieldSet.Fields?.Select(_ => _.Id).ToList();

                var missingFieldIds = dtoFieldIds == null ? 
                    dbFieldIds : dbFieldIds.Except(dtoFieldIds).ToList();

                if (missingFieldIds.Any())
                {
                    // FieldSet[i].Fields
                    var idx = dtoFieldSets.IndexOf(dtoFieldSet);
                    var key = $"{nameof(CreateAssetDto.FieldSets)}[{idx}].{nameof(CreateAssetFieldSetDto.Fields)}";

                    var messages = missingFieldIds.Select(_ => $"Field with id {_} is missing.").ToList();
                    validationErrors.Add(key, messages);
                }
            }

            if (validationErrors.Any())
            {
                throw new ValidationException(validationErrors);
            }

            // Step 5: ensure field data validations pass
            var dbFields = await GetFieldsAsync(dbFieldSets, cancellationToken);

            foreach (var dbFieldSet in dbFieldSets)
            {
                var dbFieldIds = dbFieldSet.FieldSetFields.Select(_ => _.FieldId).ToList();
                var dbFieldSetFields = dbFields.Where(_ => dbFieldIds.Contains(_.Id)).ToList();
                
                var dtoFieldSet = dtoFieldSets.First(_ => _.Id.Equals(dbFieldSet.Id));

                foreach (var dbField in dbFieldSetFields)
                {
                    var dtoField = dtoFieldSet.Fields.First(_ => _.Id.Equals(dbField.Id));
                    var dbFieldSetField = dbFieldSet.FieldSetFields.First(_ => _.FieldId.Equals(dbField.Id));

                    var providedDataType = dtoField.Data.Type;
                    var expectedDataType = _fieldDataConverter.GetMatchingDataType(dbField.Properties);

                    // FieldSet[idx].Fields[jdx].Data
                    var idx = dtoFieldSets.IndexOf(dtoFieldSet);
                    var jdx = dtoFieldSet.Fields.IndexOf(dtoField);

                    var key =
                        $"{nameof(CreateAssetDto.FieldSets)}[{idx}]." +
                        $"{nameof(CreateAssetFieldSetDto.Fields)}[{jdx}]." +
                        $"{nameof(CreateAssetFieldDto.Data)}";
                    
                    if (providedDataType != expectedDataType)
                    {
                        key = $"{key}.Type";

                        validationErrors.Add(key, new List<string>{$"Invalid data type '{providedDataType}', expected '{expectedDataType}'."});
                    }
                    else
                    {
                        var fieldPropertiesResolver = new FieldPropertiesResolver(_mapper, dbField.Properties, dbFieldSetField.IsRequired);
                        var validator = (IValidator) new UpsertAssetFieldDtoFieldValidator(fieldPropertiesResolver);

                        var context = ValidationContext<object>.CreateWithOptions(dtoField,
                            opt => opt.IncludeAllRuleSets());
            
                        var validationResult = await validator.ValidateAsync(context, cancellationToken);
                        
                        if (!validationResult.IsValid && validationResult.Errors.Any())
                        {
                            key = $"{key}.Value";
                            
                            foreach (var error in validationResult.Errors)
                            {
                                if (validationErrors.ContainsKey(key))
                                {
                                    validationErrors[key].Add(error.ErrorMessage);
                                }
                                else
                                {
                                    validationErrors.Add(key, new List<string>{error.ErrorMessage});
                                }
                            }
                        }
                    }
                }
            }

            if (validationErrors.Any())
            {
                throw new ValidationException(validationErrors);
            }

            return (dbFieldSets, dbFields);
        }
    }
}