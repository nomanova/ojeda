using System;
using System.Collections.Generic;
using System.Linq;
using NomaNova.Ojeda.Api.Tests.Helpers;
using NomaNova.Ojeda.Core.Domain.Assets;
using NomaNova.Ojeda.Models.Dtos.Assets;

namespace NomaNova.Ojeda.Api.Tests.Factories
{
    public static class AssetFactory
    {
        public static Asset NewRandom(string assetTypeId, string id = null, string assetId = null)
        {
            return new Asset
            {
                Id = id ?? Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = FactoryHelper.RandomString(),
                AssetId = assetId ?? "0",
                AssetTypeId = assetTypeId
            };
        }

        public static CreateAssetDto NewRandomCreateDto(
            string assetTypeId, string assetId = null, Dictionary<string, ICollection<string>> ids = null)
        {
            var createAssetDto = new CreateAssetDto
            {
                Name = FactoryHelper.RandomString(),
                AssetTypeId = assetTypeId,
                AssetId = assetId ?? "0",
                FieldSets = new List<CreateAssetFieldSetDto>()
            };

            if (ids == null)
            {
                return createAssetDto;
            }

            foreach (var (key, value) in ids)
            {
                createAssetDto.FieldSets.Add(new CreateAssetFieldSetDto
                {
                    Id = key,
                    Fields = value.Select(_ => new CreateAssetFieldDto
                    {
                        Id = _,
                        Data = new FieldDataDto()
                    }).ToList()
                });
            }

            return createAssetDto;
        }
        
        public static UpdateAssetDto NewRandomUpdateDto(
            string assetTypeId, string assetId = null, Dictionary<string, ICollection<string>> ids = null)
        {
            var updateAssetDto = new UpdateAssetDto
            {
                Name = FactoryHelper.RandomString(),
                AssetTypeId = assetTypeId,
                AssetId = assetId ?? "0",
                FieldSets = new List<UpdateAssetFieldSetDto>()
            };

            if (ids == null)
            {
                return updateAssetDto;
            }

            foreach (var (key, value) in ids)
            {
                updateAssetDto.FieldSets.Add(new UpdateAssetFieldSetDto
                {
                    Id = key,
                    Fields = value.Select(_ => new UpdateAssetFieldDto
                    {
                        Id = _,
                        Data = new FieldDataDto()
                    }).ToList()
                });
            }

            return updateAssetDto;
        }
    }
}