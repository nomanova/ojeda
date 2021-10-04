using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Data.Context.Converters;
using NomaNova.Ojeda.Utils.Services.Interfaces;

namespace NomaNova.Ojeda.Data.Context.Configurations
{
    public class FieldEntityTypeConfiguration : IEntityTypeConfiguration<Field>
    {
        private readonly ISerializer _serializer;
        private readonly IList<JsonConverter> _jonConverters;
        
        public FieldEntityTypeConfiguration(ISerializer serializer)
        {
            _serializer = serializer;
            _jonConverters = new List<JsonConverter>{new FieldDataJsonConverter()};
        }

        public void Configure(EntityTypeBuilder<Field> builder)
        {
            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.Data).HasConversion(
                fieldData => _serializer.Serialize(fieldData, _jonConverters),
                fieldData => _serializer.Deserialize<FieldData>(fieldData, _jonConverters));
        }
    }
}