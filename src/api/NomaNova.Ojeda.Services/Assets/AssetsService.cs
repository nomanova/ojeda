using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Api.Exceptions;
using NomaNova.Ojeda.Core.Domain.AssetClasses;
using NomaNova.Ojeda.Core.Domain.Assets;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models;
using NomaNova.Ojeda.Models.Assets;

namespace NomaNova.Ojeda.Services.Assets
{
    public class AssetsService : BaseService, IAssetsService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Asset> _assetsRepository;
        private readonly IRepository<AssetClass> _assetClassesRepository;
        private readonly IRepository<FieldSet> _fieldSetsRepository;
        private readonly IRepository<Field> _fieldsRepository;

        public AssetsService(
            IMapper mapper,
            IRepository<Asset> assetsRepository,
            IRepository<AssetClass> assetClassesRepository,
            IRepository<FieldSet> fieldSetsRepository,
            IRepository<Field> fieldsRepository)
        {
            _mapper = mapper;
            _assetsRepository = assetsRepository;
            _assetClassesRepository = assetClassesRepository;
            _fieldSetsRepository = fieldSetsRepository;
            _fieldsRepository = fieldsRepository;
        }

        public async Task<AssetDto> GetByAssetClassAsync(string assetClassId, CancellationToken cancellationToken)
        {
            var assetClass = await _assetClassesRepository.GetByIdAsync(assetClassId, query =>
            {
                return query
                    .Include(s => s.AssetClassFieldSets);
            }, cancellationToken);
            
            if (assetClass == null)
            {
                throw new NotFoundException();
            }
            
            return await AssetClassToAssetDto(assetClass, null, cancellationToken);
        }

        public async Task<AssetDto> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var asset = await _assetsRepository.GetByIdAsync(id, query =>
            {
                return query
                    .Include(s => s.AssetClass)
                    .ThenInclude(f => f.AssetClassFieldSets)
                    .Include(a => a.FieldValues);
            }, cancellationToken);
            
            if (asset == null)
            {
                throw new NotFoundException();
            }

            var assetClass = asset.AssetClass;
            var fieldValues = asset.FieldValues;

            var assetDto = await AssetClassToAssetDto(assetClass, (fieldSetId, fieldId, fieldType) =>
            {
                var fieldValue = fieldValues.FirstOrDefault(_ => _.FieldSetId.Equals(fieldSetId) && _.FieldId.Equals(fieldId));

                var byteValue = fieldValue?.Value;

                if (byteValue == null || byteValue.Length == 0)
                {
                    return null;
                }

                return Convert.ToBase64String(byteValue);
            }, cancellationToken);
            
            assetDto.Id = id;

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
                searchQuery, null, orderBy, orderAsc, pageNumber, pageSize, cancellationToken);

            var paginatedAssetsDto = _mapper.Map<PaginatedListDto<AssetSummaryDto>>(paginatedAssets);
            paginatedAssetsDto.Items = paginatedAssets.Select(f => _mapper.Map<AssetSummaryDto>(f)).ToList();

            return paginatedAssetsDto;
        }
        
        public async Task<AssetDto> CreateAsync(AssetDto assetDto, CancellationToken cancellationToken)
        {
            await Validate(null, assetDto, cancellationToken);
            
            var asset = _mapper.Map<Asset>(assetDto);
            asset.Id = Guid.NewGuid().ToString();

            asset = await _assetsRepository.InsertAsync(asset, cancellationToken);

            return _mapper.Map<AssetDto>(asset);
        }

        public async Task<AssetDto> UpdateAsync(string id, AssetDto assetDto,
            CancellationToken cancellationToken)
        {
            var asset = await _assetsRepository.GetByIdAsync(id, cancellationToken);

            if (asset == null)
            {
                throw new NotFoundException();
            }

            await Validate(id, assetDto, cancellationToken);

            asset = _mapper.Map(assetDto, asset);
            asset.Id = id;
            
            asset = await _assetsRepository.UpdateAsync(asset, cancellationToken);

            return _mapper.Map<AssetDto>(asset);
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
        
        private async Task Validate(string id, AssetDto assetDto, CancellationToken cancellationToken)
        {
            assetDto.Id = id;
            await Validate(new AssetDtoBusinessValidator(_assetClassesRepository), assetDto, cancellationToken);
        }
        
        private async Task<AssetDto> AssetClassToAssetDto(
            AssetClass assetClass, 
            Func<string, string, FieldType, string> fieldValueResolver, 
            CancellationToken cancellationToken)
        {
            // Field sets
            var fieldSetIds = assetClass.AssetClassFieldSets
                .Select(_ => _.FieldSetId)
                .ToList();

            var fieldSets = await _fieldSetsRepository.GetAllAsync(query =>
            {
                return query
                    .Where(_ => fieldSetIds.Contains(_.Id))
                    .Include(_ => _.FieldSetFields);
            }, cancellationToken);
            
            // Fields
            var fieldIds = fieldSets
                .SelectMany(_ => _.FieldSetFields)
                .Select(_ => _.FieldId)
                .ToList();

            var fields = await _fieldsRepository.GetAllAsync(query =>
            {
                return query.Where(_ => fieldIds.Contains(_.Id));
            }, cancellationToken);
            
            // Dto
            var assetDto = new AssetDto
            {
                Id = null,
                AssetClassId = assetClass.Id
            };

            foreach (var assetClassFieldSet in assetClass.AssetClassFieldSets.OrderBy(_ => _.Order))
            {
                var fieldSetId = assetClassFieldSet.FieldSetId;
                var fieldSet = fieldSets.FirstOrDefault(_ => _.Id.Equals(fieldSetId));
                
                if (fieldSet == null)
                {
                    throw new Exception($"Field set {fieldSetId} does not belong to asset class {assetClassFieldSet.AssetClassId}");
                }
                
                var fieldSetDto = _mapper.Map<AssetFieldSetDto>(fieldSet);
                fieldSetDto.Order = assetClassFieldSet.Order;

                foreach (var fieldSetField in fieldSet.FieldSetFields.OrderBy(_ => _.Order))
                {
                    var fieldId = fieldSetField.FieldId;
                    var field = fields.FirstOrDefault(_ => _.Id.Equals(fieldId));
                    
                    if (field == null)
                    {
                        throw new Exception($"Field {fieldId} does not belong to field set {fieldSetField.FieldSetId}");
                    }

                    var fieldDto = _mapper.Map<AssetFieldDto>(field);
                    fieldDto.Order = fieldSetField.Order;
                    
                    // Value
                    if (fieldValueResolver != null)
                    {
                        fieldDto.Value = fieldValueResolver(fieldSetId, fieldId, field.Type);
                    }

                    fieldSetDto.Fields.Add(fieldDto);
                }
                
                assetDto.FieldSets.Add(fieldSetDto);
            }

            return assetDto;
        }
    }
}