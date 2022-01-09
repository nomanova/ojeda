using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Core.Domain.AssetIdTypes;
using NomaNova.Ojeda.Utils.Services.Interfaces;

namespace NomaNova.Ojeda.Data.Seeders;

public static class AssetIdTypesSeeder
{
    private static readonly List<AssetIdType> AssetIdTypes = new()
    {
        new AssetIdType
        {
            Id = "79c1a519-a683-4306-b5e7-e89b0da0a1b5",
            Name = "Default",
            Description = "",
            WithManualEntry = true,
            Properties = new Ean13SymbologyProperties()
        }
    };
    
    public static async Task Seed(DbContext context, ITimeKeeper timeKeeper)
    {
        foreach (var assetIdType in AssetIdTypes)
        {
            await UpsertAssetIdType(context, timeKeeper, assetIdType);
        }
    }

    private static async Task UpsertAssetIdType(DbContext context, ITimeKeeper timeKeeper, AssetIdType assetIdType)
    {
        var dbAssetIdType = await context.Set<AssetIdType>().FirstOrDefaultAsync(_ => _.Name.Equals(assetIdType.Name));
        var now = timeKeeper.UtcNow;

        if (dbAssetIdType == null)
        {
            assetIdType.CreatedAt = now;
            assetIdType.UpdatedAt = now;

            await context.Set<AssetIdType>().AddAsync(assetIdType);
        }
        else
        {
            dbAssetIdType.Description = assetIdType.Description;
            dbAssetIdType.Properties = assetIdType.Properties;

            dbAssetIdType.UpdatedAt = now;
            
            context.Set<AssetIdType>().Update(dbAssetIdType);
        }

        await context.SaveChangesAsync();
    }
}