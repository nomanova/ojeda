using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Api.Exceptions;
using NomaNova.Ojeda.Core.Domain.Assets;
using NomaNova.Ojeda.Core.Domain.AssetTypes;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Dtos.AssetTypes;
using NomaNova.Ojeda.Models.Shared;
using NomaNova.Ojeda.Services.AssetTypes.Interfaces;
using NomaNova.Ojeda.Services.AssetTypes.Validators;

namespace NomaNova.Ojeda.Services.AssetTypes
{
    public class AssetTypesService : BaseService, IAssetTypesService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<FieldSet> _fieldSetsRepository;
        private readonly IRepository<AssetType> _assetTypesRepository;
        private readonly IRepository<Asset> _assetsRepository;

        public AssetTypesService(
            IMapper mapper,
            IRepository<FieldSet> fieldSetsRepository,
            IRepository<AssetType> assetTypesRepository,
            IRepository<Asset> assetsRepository)
        {
            _mapper = mapper;
            _fieldSetsRepository = fieldSetsRepository;
            _assetTypesRepository = assetTypesRepository;
            _assetsRepository = assetsRepository;
        }

        public async Task<AssetTypeDto> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var assetType = await _assetTypesRepository.GetByIdAsync(id, query =>
            {
                return query
                    .Include(c => c.AssetTypeFieldSets)
                    .ThenInclude(f => f.FieldSet);
            }, cancellationToken);

            if (assetType == null)
            {
                throw new NotFoundException();
            }

            var assetTypeDto = _mapper.Map<AssetTypeDto>(assetType);

            assetTypeDto.FieldSets = assetTypeDto.FieldSets
                .OrderBy(_ => _.Order)
                .ThenBy(_ => _.FieldSet.Name)
                .ToList();

            return assetTypeDto;
        }

        public async Task<PaginatedListDto<AssetTypeDto>> GetAsync(
            string searchQuery,
            string orderBy,
            bool orderAsc,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = nameof(FieldSet.Name);
            }
            
            var paginatedAssetTypes = await _assetTypesRepository.GetAllPaginatedAsync(
                searchQuery, query =>
                {
                    return query
                        .Include(s => s.AssetTypeFieldSets)
                        .ThenInclude(f => f.FieldSet);
                }, orderBy, orderAsc, pageNumber, pageSize, cancellationToken);
            
            var paginatedAssetTypesDto = _mapper.Map<PaginatedListDto<AssetTypeDto>>(paginatedAssetTypes);
            paginatedAssetTypesDto.Items = paginatedAssetTypes.Select(f => _mapper.Map<AssetTypeDto>(f)).ToList();

            return paginatedAssetTypesDto;
        }
        
        public async Task<AssetTypeDto> CreateAsync(
            CreateAssetTypeDto assetTypeDto, CancellationToken cancellationToken)
        {
            await ValidateAndThrowAsync(new CreateAssetTypeDtoBusinessValidator(_fieldSetsRepository, _assetTypesRepository), assetTypeDto, cancellationToken);

            var assetTypeId = Guid.NewGuid().ToString();

            var assetType = _mapper.Map<AssetType>(assetTypeDto);
            assetType.Id = assetTypeId;
            
            assetType.AssetTypeFieldSets = assetTypeDto.FieldSets.Select(f => _mapper.Map<AssetTypeFieldSet>(f, opt =>
                opt.AfterMap((_, dest) =>
                {
                    dest.AssetTypeId = assetTypeId;
                }))).ToList();

            assetType = await _assetTypesRepository.InsertAsync(assetType, cancellationToken);
            
            return _mapper.Map<AssetTypeDto>(assetType);
        }
        
        public async Task<AssetTypeDto> UpdateAsync(
            string id, UpdateAssetTypeDto assetTypeDto, CancellationToken cancellationToken)
        {
            var assetType = await PrepareUpdateAsync(id, assetTypeDto, cancellationToken);

            assetType = _mapper.Map(assetTypeDto, assetType);
            assetType.Id = id;

            assetType.AssetTypeFieldSets = assetTypeDto.FieldSets.Select(f => _mapper.Map<AssetTypeFieldSet>(f, opt =>
                opt.AfterMap((_, dest) => dest.AssetTypeId = id))).ToList();

            assetType = await _assetTypesRepository.UpdateAsync(assetType, cancellationToken);

            return _mapper.Map<AssetTypeDto>(assetType);
        }
        
        public async Task<DryRunUpdateAssetTypeDto> DryRunUpdateAsync(
            string id, UpdateAssetTypeDto assetTypeDto, CancellationToken cancellationToken = default)
        {
            var assetType = await PrepareUpdateAsync(id, assetTypeDto, cancellationToken);
            
            // Determine removed field sets
            var dbFieldSetIds = assetType.AssetTypeFieldSets.Select(_ => _.FieldSetId).ToList();
            var dtoFieldSetIds = assetTypeDto.FieldSets.Select(_ => _.Id).ToList();
            
            var removedFieldSetIds = dbFieldSetIds.Where(db => dtoFieldSetIds.All(dto => dto != db)).ToList();

            // Fetch impacted assets
            var assets = await _assetsRepository.GetAllAsync(query =>
            {
                return query.Where(_ =>
                    _.FieldValues.Any(fv => 
                                            removedFieldSetIds.Contains(fv.FieldSetId) &&
                                            fv.Value != null)
                );
            }, cancellationToken);
            
            // Map dto
            var updateAssetTypeDto = new DryRunUpdateAssetTypeDto
            {
                Assets = assets.Select(_ => _mapper.Map<NamedEntityDto>(_)).ToList()
            };
            
            return updateAssetTypeDto;
        }

        public async Task<DryRunDeleteAssetTypeDto> DeleteAsync(string id, bool dryRun, CancellationToken cancellationToken)
        {
            var assetType = await _assetTypesRepository.GetByIdAsync(id, cancellationToken);

            if (assetType == null)
            {
                throw new NotFoundException();
            }

            var deleteAssetTypeDto = new DryRunDeleteAssetTypeDto();
            
            // Fetch impacted assets
            var assets = await _assetsRepository.GetAllAsync(query =>
            {
                return query.Where(_ => _.AssetTypeId == id);
            }, cancellationToken);

            deleteAssetTypeDto.Assets = assets.Select(_ => _mapper.Map<NamedEntityDto>(_)).ToList();
            
            if (dryRun)
            {
                return deleteAssetTypeDto;
            }

            await _assetTypesRepository.DeleteAsync(assetType, cancellationToken);
            return deleteAssetTypeDto;
        }

        private async Task<AssetType> PrepareUpdateAsync(
            string id, UpdateAssetTypeDto assetTypeDto, CancellationToken cancellationToken)
        {
            var assetType = await _assetTypesRepository.GetByIdAsync(id, query =>
            {
                return query.Include(s => s.AssetTypeFieldSets);
            }, cancellationToken);

            if (assetType == null)
            {
                throw new NotFoundException();
            }

            await ValidateAndThrowAsync(new UpdateAssetTypeDtoBusinessValidator(
                _fieldSetsRepository, _assetTypesRepository, id), assetTypeDto, cancellationToken);

            return assetType;
        }
    }
}