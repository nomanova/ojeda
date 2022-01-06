using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using NomaNova.Ojeda.Core.Domain.AssetIdTypes;
using NomaNova.Ojeda.Core.Domain.AssetTypes;
using NomaNova.Ojeda.Core.Exceptions;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Dtos.AssetIdTypes;
using NomaNova.Ojeda.Models.Shared;
using NomaNova.Ojeda.Services.Features.AssetIdTypes.Interfaces;
using NomaNova.Ojeda.Services.Features.AssetIdTypes.Validators;
using NomaNova.Ojeda.Utils.Extensions;

namespace NomaNova.Ojeda.Services.Features.AssetIdTypes;

public class AssetIdTypesService : BaseEntityService<AssetIdType>, IAssetIdTypesService
{
    private readonly IRepository<AssetType> _assetTypesRepository;

    public AssetIdTypesService(
        IMapper mapper,
        IRepository<AssetIdType> assetIdTypesRepository,
        IRepository<AssetType> assetTypesRepository) : base(mapper, assetIdTypesRepository)
    {
        _assetTypesRepository = assetTypesRepository;
    }

    public async Task<AssetIdTypeDto> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await GetByIdAsync<AssetIdTypeDto>(id, cancellationToken);
    }

    public async Task<PaginatedListDto<AssetIdTypeDto>> GetAsync(
        string searchQuery,
        string orderBy,
        bool orderAsc,
        IList<string> excludedIds,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await GetAsync<AssetIdTypeDto>(searchQuery, orderBy, orderAsc, excludedIds, pageNumber, pageSize,
            cancellationToken);
    }

    public async Task<AssetIdTypeDto> CreateAsync(CreateAssetIdTypeDto assetIdTypeDto,
        CancellationToken cancellationToken = default)
    {
        await ValidateAndThrowAsync(new CreateAssetIdTypeBusinessValidator(Repository), assetIdTypeDto,
            cancellationToken);

        var assetIdType = Mapper.Map<AssetIdType>(assetIdTypeDto);
        assetIdType.Id = Guid.NewGuid().ToString();

        assetIdType = await Repository.InsertAsync(assetIdType, cancellationToken);

        return Mapper.Map<AssetIdTypeDto>(assetIdType);
    }

    public async Task<AssetIdTypeDto> UpdateAsync(string id, UpdateAssetIdTypeDto assetIdTypeDto,
        CancellationToken cancellationToken = default)
    {
        var assetIdType = await Repository.GetByIdAsync(id, cancellationToken);

        if (assetIdType == null)
        {
            throw new NotFoundException();
        }

        await ValidateAndThrowAsync(new UpdateAssetIdTypeBusinessValidator(Repository, id), assetIdTypeDto,
            cancellationToken);

        assetIdType = Mapper.Map(assetIdTypeDto, assetIdType);
        assetIdType.Id = id;
        assetIdType.Properties = Mapper.Map<SymbologyProperties>(assetIdTypeDto.Properties);

        assetIdType = await Repository.UpdateAsync(assetIdType, cancellationToken);

        return Mapper.Map<AssetIdTypeDto>(assetIdType);
    }

    public async Task<DryRunDeleteAssetIdTypeDto> DeleteAsync(string id, bool dryRun,
        CancellationToken cancellationToken = default)
    {
        var assetIdType = await Repository.GetByIdAsync(id, cancellationToken);

        if (assetIdType == null)
        {
            throw new NotFoundException();
        }

        var deleteAssetIdTypeDto = new DryRunDeleteAssetIdTypeDto();

        // Fetch impacted asset types
        var assetTypes = await _assetTypesRepository.GetAllAsync(query =>
        {
            return query.Where(_ => _.AssetIdTypeId == id);
        }, cancellationToken);

        deleteAssetIdTypeDto.AssetTypes = assetTypes.Select(_ => Mapper.Map<NamedEntityDto>(_)).ToList();

        if (dryRun)
        {
            return deleteAssetIdTypeDto;
        }

        // Do not proceed when asset types are still linked to this id type
        if (assetTypes.HasElements())
        {
            throw new ValidationException(
                nameof(AssetIdType.Id), "Cannot delete item as it is in use.");
        }

        await Repository.DeleteAsync(assetIdType, cancellationToken);
        return deleteAssetIdTypeDto;
    }
}