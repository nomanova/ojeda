using System;
using System.Linq;
using NomaNova.Ojeda.Api.Tests.Helpers;
using NomaNova.Ojeda.Core.Domain.AssetTypes;
using NomaNova.Ojeda.Models.Dtos.AssetTypes;

namespace NomaNova.Ojeda.Api.Tests.Factories
{
    public static class AssetTypeFactory
    {
        public static AssetType NewRandom(string id = null)
        {
            return new AssetType
            {
                Id = id ?? Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = FactoryHelper.RandomString(),
                Description = FactoryHelper.RandomString()
            };
        }

        public static CreateAssetTypeDto NewRandomCreateDto(string assetIdTypeId, params string[] fieldSetIds)
        {
            return new CreateAssetTypeDto
            {
                Name = FactoryHelper.RandomString(),
                Description = FactoryHelper.RandomString(),
                AssetIdTypeId = assetIdTypeId,
                FieldSets = fieldSetIds.Select(id => new CreateAssetTypeFieldSetDto
                {
                    Id = id
                }).ToList()
            };
        }
        
        public static UpdateAssetTypeDto NewRandomUpdateDto(string assetIdTypeId, params string[] fieldSetIds)
        {
            return new UpdateAssetTypeDto
            {
                Name = FactoryHelper.RandomString(),
                Description = FactoryHelper.RandomString(),
                AssetIdTypeId = Guid.NewGuid().ToString(),
                FieldSets = fieldSetIds.Select(id => new UpdateAssetTypeFieldSetDto
                {
                    Id = id
                }).ToList()
            };
        }
    }
}