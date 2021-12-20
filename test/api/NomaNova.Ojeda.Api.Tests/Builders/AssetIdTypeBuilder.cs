using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Api.Tests.Factories;
using NomaNova.Ojeda.Api.Tests.Helpers;
using NomaNova.Ojeda.Core.Domain.AssetIdTypes;

namespace NomaNova.Ojeda.Api.Tests.Builders;

public class AssetIdTypeBuilder
{
    private readonly AssetIdType _assetIdType;

    public AssetIdTypeBuilder(string id = null)
    {
        _assetIdType = AssetIdTypeFactory.NewRandom(id);
    }

    public AssetIdTypeBuilder WithName(string name)
    {
        _assetIdType.Name = name;
        return this;
    }

    public AssetIdTypeBuilder WithDescription(string description)
    {
        _assetIdType.Description = description;
        return this;
    }

    public AssetIdTypeBuilder WithProperties(SymbologyProperties properties)
    {
        _assetIdType.Properties = properties;
        return this;
    }

    public async Task<AssetIdType> Build(DbContext context)
    {
        await DatabaseHelper.Add(context, _assetIdType);
        return _assetIdType;
    }
}