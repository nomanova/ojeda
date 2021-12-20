using System;
using NomaNova.Ojeda.Api.Tests.Helpers;
using NomaNova.Ojeda.Core.Domain.AssetIdTypes;
using NomaNova.Ojeda.Models.Dtos.AssetIdTypes;

namespace NomaNova.Ojeda.Api.Tests.Factories;

public static class AssetIdTypeFactory
{
    public static AssetIdType NewRandom(string id = null)
    {
        return new AssetIdType
        {
            Id = id ?? Guid.NewGuid().ToString(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Name = FactoryHelper.RandomString(),
            Description = FactoryHelper.RandomString(),
            Properties = new Ean13SymbologyProperties()
        };
    }

    public static CreateAssetIdTypeDto NewRandomCreateDto()
    {
        return new CreateAssetIdTypeDto
        {
            Name = FactoryHelper.RandomString(),
            Description = FactoryHelper.RandomString(),
            Properties = new Ean13SymbologyPropertiesDto()
        };
    }

    public static UpdateAssetIdTypeDto NewRandomUpdateDto()
    {
        return new UpdateAssetIdTypeDto
        {
            Name = FactoryHelper.RandomString(),
            Description = FactoryHelper.RandomString(),
            Properties = new Ean13SymbologyPropertiesDto()
        };
    }
}