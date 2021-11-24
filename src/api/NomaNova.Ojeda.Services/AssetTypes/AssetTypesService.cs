using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Api.Exceptions;
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

        public AssetTypesService(
            IMapper mapper,
            IRepository<FieldSet> fieldSetsRepository,
            IRepository<AssetType> assetTypesRepository)
        {
            _mapper = mapper;
            _fieldSetsRepository = fieldSetsRepository;
            _assetTypesRepository = assetTypesRepository;
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

            assetType = _mapper.Map(assetTypeDto, assetType);
            assetType.Id = id;

            assetType.AssetTypeFieldSets = assetTypeDto.FieldSets.Select(f => _mapper.Map<AssetTypeFieldSet>(f, opt =>
                opt.AfterMap((_, dest) => dest.AssetTypeId = id))).ToList();

            assetType = await _assetTypesRepository.UpdateAsync(assetType, cancellationToken);

            return _mapper.Map<AssetTypeDto>(assetType);
        }
        
        public async Task DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var assetType = await _assetTypesRepository.GetByIdAsync(id, cancellationToken);

            if (assetType == null)
            {
                throw new NotFoundException();
            }
            
            await _assetTypesRepository.DeleteAsync(assetType, cancellationToken);
        }
    }
}