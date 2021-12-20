using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using NomaNova.Ojeda.Core.Domain.AssetIdTypes;
using NomaNova.Ojeda.Data.Context.Converters;
using NomaNova.Ojeda.Utils.Services.Interfaces;

namespace NomaNova.Ojeda.Data.Context.Configurations;

public class AssetIdTypeEntityTypeConfiguration : IEntityTypeConfiguration<AssetIdType>
{
    private readonly ISerializer _serializer;
    private readonly IList<JsonConverter> _jonConverters;
        
    public AssetIdTypeEntityTypeConfiguration(ISerializer serializer)
    {
        _serializer = serializer;
        _jonConverters = new List<JsonConverter>{new SymbologyPropertiesJsonConverter()};
    }

    public void Configure(EntityTypeBuilder<AssetIdType> builder)
    {
        builder.HasKey(_ => _.Id);
        
        builder.Property(_ => _.Properties).HasConversion(
            properties => _serializer.Serialize(properties, _jonConverters),
            properties => _serializer.Deserialize<SymbologyProperties>(properties, _jonConverters));
    }
}