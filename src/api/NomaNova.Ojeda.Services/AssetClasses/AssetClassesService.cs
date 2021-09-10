using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Api.Exceptions;
using NomaNova.Ojeda.Core.Domain.AssetClasses;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models;
using NomaNova.Ojeda.Models.AssetClasses;

namespace NomaNova.Ojeda.Services.AssetClasses
{
    public class AssetClassesService : BaseService, IAssetClassesService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<FieldSet> _fieldSetsRepository;
        private readonly IRepository<AssetClass> _assetClassesRepository;

        public AssetClassesService(
            IMapper mapper,
            IRepository<FieldSet> fieldSetsRepository,
            IRepository<AssetClass> assetClassesRepository)
        {
            _mapper = mapper;
            _fieldSetsRepository = fieldSetsRepository;
            _assetClassesRepository = assetClassesRepository;
        }

        public async Task<AssetClassDto> GetAssetClassByIdAsync(string id, CancellationToken cancellationToken)
        {
            var assetClass = await _assetClassesRepository.GetByIdAsync(id, query =>
            {
                return query
                    .Include(c => c.AssetClassFieldSets)
                    .ThenInclude(f => f.FieldSet);
            }, cancellationToken);

            if (assetClass == null)
            {
                throw new NotFoundException();
            }

            return _mapper.Map<AssetClassDto>(assetClass);
        }

        public async Task<PaginatedListDto<AssetClassDto>> GetAssetClassesAsync(
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
            
            var paginatedAssetClasses = await _assetClassesRepository.GetAllPaginatedAsync(
                searchQuery, query =>
                {
                    return query
                        .Include(s => s.AssetClassFieldSets)
                        .ThenInclude(f => f.FieldSet);
                }, orderBy, orderAsc, pageNumber, pageSize, cancellationToken);
            
            var paginatedAssetClassesDto = _mapper.Map<PaginatedListDto<AssetClassDto>>(paginatedAssetClasses);
            paginatedAssetClassesDto.Items = paginatedAssetClasses.Select(f => _mapper.Map<AssetClassDto>(f)).ToList();

            return paginatedAssetClassesDto;
        }
        
        public async Task<AssetClassDto> CreateAssetClassAsync(
            AssetClassDto assetClassDto, CancellationToken cancellationToken)
        {
            await Validate(null, assetClassDto, cancellationToken);

            var assetClassId = Guid.NewGuid().ToString();

            var assetClass = _mapper.Map<AssetClass>(assetClassDto);
            assetClass.Id = assetClassId;
            
            assetClass.AssetClassFieldSets = assetClassDto.FieldSets.Select(f => _mapper.Map<AssetClassFieldSet>(f, opt =>
                opt.AfterMap((_, dest) =>
                {
                    dest.AssetClassId = assetClassId;
                }))).ToList();

            assetClass = await _assetClassesRepository.InsertAsync(assetClass, cancellationToken);
            
            return _mapper.Map<AssetClassDto>(assetClass);
        }
        
        public async Task<AssetClassDto> UpdateAssetClassAsync(
            string id, AssetClassDto assetClassDto, CancellationToken cancellationToken)
        {
            var assetClass = await _assetClassesRepository.GetByIdAsync(id, query =>
            {
                return query.Include(s => s.AssetClassFieldSets);
            }, cancellationToken);

            if (assetClass == null)
            {
                throw new NotFoundException();
            }

            await Validate(id, assetClassDto, cancellationToken);

            assetClass = _mapper.Map(assetClassDto, assetClass);
            assetClass.Id = id;

            assetClass.AssetClassFieldSets = assetClassDto.FieldSets.Select(f => _mapper.Map<AssetClassFieldSet>(f, opt =>
                opt.AfterMap((_, dest) => dest.AssetClassId = id))).ToList();

            assetClass = await _assetClassesRepository.UpdateAsync(assetClass, cancellationToken);

            return _mapper.Map<AssetClassDto>(assetClass);
        }
        
        public async Task DeleteAssetClassAsync(string id, CancellationToken cancellationToken)
        {
            var assetClass = await _assetClassesRepository.GetByIdAsync(id, cancellationToken);

            if (assetClass == null)
            {
                throw new NotFoundException();
            }
            
            await _assetClassesRepository.DeleteAsync(assetClass, cancellationToken);
        }

        private async Task Validate(string id, AssetClassDto assetClassDto, CancellationToken cancellationToken)
        {
            assetClassDto.Id = id;
            await Validate(new AssetClassDtoBusinessValidator(_fieldSetsRepository, _assetClassesRepository), assetClassDto, cancellationToken);
        }
    }
}