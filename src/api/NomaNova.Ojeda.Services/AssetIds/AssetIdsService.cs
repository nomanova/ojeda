using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Core.Domain.AssetIdTypes;
using NomaNova.Ojeda.Core.Domain.Assets;
using NomaNova.Ojeda.Core.Domain.AssetTypes;
using NomaNova.Ojeda.Core.Exceptions;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Dtos.AssetIds;
using NomaNova.Ojeda.Services.AssetIds.Interfaces;
using NomaNova.Ojeda.Utils.Extensions;
using NomaNova.Ojeda.Utils.Services.Interfaces;

namespace NomaNova.Ojeda.Services.AssetIds;

public class AssetIdsService : BaseService, IAssetIdsService
{
    private readonly IRepository<Asset> _assetsRepository;
    private readonly IRepository<AssetType> _assetTypesRepository;
    private readonly ISymbologyService _symbologyService;

    public AssetIdsService(
        IRepository<Asset> assetsRepository,
        IRepository<AssetType> assetTypesRepository,
        ISymbologyService symbologyService)
    {
        _assetsRepository = assetsRepository;
        _assetTypesRepository = assetTypesRepository;
        _symbologyService = symbologyService;
    }

    public async Task<GenerateAssetIdDto> GenerateAssetId(string assetTypeId,
        CancellationToken cancellationToken = default)
    {
        // Fetch asset id type
        var assetType = await _assetTypesRepository.GetByIdAsync(assetTypeId,
            query => { return query.Include(_ => _.AssetIdType); }, cancellationToken);

        if (assetType == null)
        {
            throw new NotFoundException();
        }

        var assetIdType = assetType.AssetIdType;

        // Generate
        var assetIds = await GetExistingAssetIds(cancellationToken);
        var (nextAssetId, nextFullAssetId) = GenerateNextAssetId(assetIds, assetIdType);

        return new GenerateAssetIdDto
        {
            AssetId = nextAssetId,
            FullAssetId = nextFullAssetId
        };
    }

    public async Task<(string assetId, string fullAssetId)> GenerateAssetId(AssetIdType assetIdType, CancellationToken cancellationToken = default)
    {
        if (assetIdType == null)
        {
            throw new ArgumentNullException(nameof(assetIdType));
        }

        var assetIds = await GetExistingAssetIds(cancellationToken);

        return GenerateNextAssetId(assetIds, assetIdType);
    }

    /**
     * Asset Id generation algorithm
     * Input:
     *  - Set of all existing asset ids.
     *  - The symbology of the desired asset id.
     *
     * 1. Iterate all existing asset ids.
     * 2. Check if the asset id matches the desired symbology.
     *  If not: continue.
     *  If yes:
     *      - Calculate the next asset id.
     *      - Check if the next asset id is already contained in the existing asset ids.
     *          If yes: continue.
     *          If no: terminate and return.
     * 3. If no match was found, none of the existing asset id's have the desired symbology,
     *    so return default.
     */
    private (string assetId, string assetIdFull) GenerateNextAssetId(List<string> assetIds, AssetIdType assetIdType)
    {
        var symbologyProperties = assetIdType.Properties;

        if (!assetIds.HasElements())
        {
            return _symbologyService.GenerateNext(null, symbologyProperties);
        }

        foreach (var assetId in assetIds)
        {
            if (_symbologyService.IsValidFull(assetId, symbologyProperties))
            {
                var (nextAssetId, nextAssetIdFull) = _symbologyService.GenerateNext(assetId, symbologyProperties);

                if (!assetIds.Contains(nextAssetIdFull))
                {
                    return (nextAssetId, nextAssetIdFull);
                }
            }
        }

        return _symbologyService.GenerateNext(null, symbologyProperties);
    }

    private async Task<List<string>> GetExistingAssetIds(CancellationToken cancellationToken)
    {
        return await _assetsRepository.GetAllAsync(query => { return query.Select(_ => _.AssetId); },
            cancellationToken);
    }
}