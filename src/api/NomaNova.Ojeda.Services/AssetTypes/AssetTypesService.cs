using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Core.Domain.Assets;
using NomaNova.Ojeda.Core.Domain.AssetTypes;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Core.Exceptions;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Dtos.AssetTypes;
using NomaNova.Ojeda.Models.Shared;
using NomaNova.Ojeda.Services.AssetTypes.Interfaces;
using NomaNova.Ojeda.Services.AssetTypes.Validators;
using NomaNova.Ojeda.Utils.Extensions;

namespace NomaNova.Ojeda.Services.AssetTypes
{
    public class AssetTypesService : BaseService<AssetType>, IAssetTypesService
    {
        private readonly IRepository<FieldSet> _fieldSetsRepository;
        private readonly IRepository<Asset> _assetsRepository;
        private readonly IRepository<FieldValue> _fieldValueRepository;

        public AssetTypesService(
            IMapper mapper,
            IRepository<FieldSet> fieldSetsRepository,
            IRepository<AssetType> assetTypesRepository,
            IRepository<Asset> assetsRepository,
            IRepository<FieldValue> fieldValueRepository) : base(mapper, assetTypesRepository)
        {
            _fieldSetsRepository = fieldSetsRepository;
            _assetsRepository = assetsRepository;
            _fieldValueRepository = fieldValueRepository;
        }

        public async Task<AssetTypeDto> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var assetType = await Repository.GetByIdAsync(id, query =>
            {
                return query
                    .Include(c => c.AssetTypeFieldSets)
                    .ThenInclude(f => f.FieldSet);
            }, cancellationToken);

            if (assetType == null)
            {
                throw new NotFoundException();
            }

            var assetTypeDto = Mapper.Map<AssetTypeDto>(assetType);
            EnsureNormalizedFieldSetOrder(assetTypeDto);

            return assetTypeDto;
        }

        public async Task<PaginatedListDto<AssetTypeDto>> GetAsync(
            string searchQuery,
            string orderBy,
            bool orderAsc,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = nameof(FieldSet.Name);
            }

            var paginatedAssetTypes = await Repository.GetAllPaginatedAsync(
                searchQuery, query =>
                {
                    return query
                        .Include(s => s.AssetTypeFieldSets)
                        .ThenInclude(f => f.FieldSet);
                }, orderBy, orderAsc, pageNumber, pageSize, cancellationToken);

            var paginatedAssetTypesDto = Mapper.Map<PaginatedListDto<AssetTypeDto>>(paginatedAssetTypes);
            paginatedAssetTypesDto.Items = paginatedAssetTypes.Select(f =>
            {
                var assetTypeDto = Mapper.Map<AssetTypeDto>(f);
                EnsureNormalizedFieldSetOrder(assetTypeDto);
                return assetTypeDto;
            }).ToList();

            return paginatedAssetTypesDto;
        }

        public async Task<AssetTypeDto> CreateAsync(
            CreateAssetTypeDto assetTypeDto, CancellationToken cancellationToken = default)
        {
            await ValidateAndThrowAsync(
                new CreateAssetTypeDtoBusinessValidator(_fieldSetsRepository, Repository), assetTypeDto,
                cancellationToken);

            var assetTypeId = Guid.NewGuid().ToString();

            var assetType = Mapper.Map<AssetType>(assetTypeDto);
            assetType.Id = assetTypeId;

            assetType.AssetTypeFieldSets = assetTypeDto.FieldSets.Select(f => Mapper.Map<AssetTypeFieldSet>(f, opt =>
                opt.AfterMap((_, dest) => { dest.AssetTypeId = assetTypeId; }))).ToList();

            assetType = await Repository.InsertAsync(assetType, cancellationToken);

            return Mapper.Map<AssetTypeDto>(assetType);
        }

        public async Task<AssetTypeDto> UpdateAsync(
            string id, UpdateAssetTypeDto assetTypeDto, CancellationToken cancellationToken = default)
        {
            var assetType = await PrepareUpdateAsync(id, assetTypeDto, cancellationToken);
            var removedFieldSetIds = GetRemovedFieldSetIds(assetType, assetTypeDto);
            
            assetType = Mapper.Map(assetTypeDto, assetType);
            assetType.Id = id;

            assetType.AssetTypeFieldSets = assetTypeDto.FieldSets.Select(f => Mapper.Map<AssetTypeFieldSet>(f, opt =>
                opt.AfterMap((_, dest) => dest.AssetTypeId = id))).ToList();

            assetType = await Repository.UpdateAsync(assetType, cancellationToken);

            // Delete any impacted field values
            if (removedFieldSetIds.HasElements())
            {
                var fieldValues = await _fieldValueRepository.GetAllAsync(query =>
                {
                    return query
                        .Include(_ => _.Asset)
                        .Where(_ => _.Asset.AssetTypeId == id && removedFieldSetIds.Contains(_.FieldSetId));
                }, cancellationToken);
                
                await _fieldValueRepository.DeleteRangeAsync(fieldValues, cancellationToken);
            }

            return Mapper.Map<AssetTypeDto>(assetType);
        }

        public async Task<DryRunUpdateAssetTypeDto> DryRunUpdateAsync(
            string id, UpdateAssetTypeDto assetTypeDto, CancellationToken cancellationToken = default)
        {
            var assetType = await PrepareUpdateAsync(id, assetTypeDto, cancellationToken);

            // Determine removed field sets
            var removedFieldSetIds = GetRemovedFieldSetIds(assetType, assetTypeDto);

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
                Assets = assets.Select(_ => Mapper.Map<NamedEntityDto>(_)).ToList()
            };

            return updateAssetTypeDto;
        }

        public async Task<DryRunDeleteAssetTypeDto> DeleteAsync(string id, bool dryRun,
            CancellationToken cancellationToken = default)
        {
            var assetType = await Repository.GetByIdAsync(id, cancellationToken);

            if (assetType == null)
            {
                throw new NotFoundException();
            }

            var deleteAssetTypeDto = new DryRunDeleteAssetTypeDto();

            // Fetch impacted assets
            var assets = await _assetsRepository.GetAllAsync(query => { return query.Where(_ => _.AssetTypeId == id); },
                cancellationToken);

            deleteAssetTypeDto.Assets = assets.Select(_ => Mapper.Map<NamedEntityDto>(_)).ToList();

            if (dryRun)
            {
                return deleteAssetTypeDto;
            }

            await Repository.DeleteAsync(assetType, cancellationToken);
            return deleteAssetTypeDto;
        }

        private async Task<AssetType> PrepareUpdateAsync(
            string id, UpdateAssetTypeDto assetTypeDto, CancellationToken cancellationToken)
        {
            var assetType = await Repository.GetByIdAsync(id,
                query => { return query.Include(s => s.AssetTypeFieldSets); }, cancellationToken);

            if (assetType == null)
            {
                throw new NotFoundException();
            }

            await ValidateAndThrowAsync(new UpdateAssetTypeDtoBusinessValidator(
                _fieldSetsRepository, Repository, id), assetTypeDto, cancellationToken);

            return assetType;
        }

        private static List<string> GetRemovedFieldSetIds(AssetType dbAssetType, UpdateAssetTypeDto dtoAssetType)
        {
            var dbFieldSetIds = dbAssetType.AssetTypeFieldSets.Select(_ => _.FieldSetId).ToList();
            var dtoFieldSetIds = dtoAssetType.FieldSets.Select(_ => _.Id).ToList();

            return dbFieldSetIds.Where(db => dtoFieldSetIds.All(dto => dto != db)).ToList();
        }

        private static void EnsureNormalizedFieldSetOrder(AssetTypeDto assetTypeDto)
        {
            assetTypeDto.FieldSets = assetTypeDto.FieldSets
                .OrderBy(_ => _.Order)
                .ThenBy(_ => _.FieldSet.Name)
                .ToList();

            for (var i = 0; i < assetTypeDto.FieldSets.Count; i++)
            {
                assetTypeDto.FieldSets[i].Order = i + 1;
            }
        }
    }
}