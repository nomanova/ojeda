using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Core.Domain.AssetTypes;
using NomaNova.Ojeda.Utils.Services.Interfaces;

namespace NomaNova.Ojeda.Data.Seeders
{
    public static class AssetTypesSeeder
    {
        public static async Task Seed(DbContext context, ITimeKeeper timeKeeper)
        {
            var assetTypes = new List<AssetType>();

            var mobilePhoneAssetType = new AssetType
            {
                Id = "fe6b1ed7-2c67-4cfa-97b6-00b559df5feb",
                Name = "Mobile Phone",
                Description = "iPhone or Android phones",
                AssetTypeFieldSets = new List<AssetTypeFieldSet>
                {
                    new() // Support
                    {
                        FieldSetId = "4aeccbc8-f0a2-42d5-a28c-11bbc891ee63",
                        AssetTypeId = "fe6b1ed7-2c67-4cfa-97b6-00b559df5feb",
                        Order = 1
                    }
                }
            };
            assetTypes.Add(mobilePhoneAssetType);
            
            
            foreach (var assetType in assetTypes)
            {
                await UpsertAssetType(context, timeKeeper, assetType);
            }
        }

        private static async Task UpsertAssetType(DbContext context, ITimeKeeper timeKeeper, AssetType assetType)
        {
            var dbAssetType = await context.Set<AssetType>().FirstOrDefaultAsync(f => f.Name.Equals(assetType.Name));
            var now = timeKeeper.UtcNow;

            if (dbAssetType == null)
            {
                assetType.CreatedAt = now;
                assetType.UpdatedAt = now;
                
                await context.Set<AssetType>().AddAsync(assetType);
            }
            else
            {
                dbAssetType.Description = assetType.Description;
                
                dbAssetType.UpdatedAt = now;
                
                context.Set<AssetType>().Update(dbAssetType);
            }

            await context.SaveChangesAsync();
        }
    }
}