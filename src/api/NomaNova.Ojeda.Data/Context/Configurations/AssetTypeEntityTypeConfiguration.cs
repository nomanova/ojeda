using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NomaNova.Ojeda.Core.Domain.AssetTypes;

namespace NomaNova.Ojeda.Data.Context.Configurations
{
    public class AssetTypeEntityTypeConfiguration : IEntityTypeConfiguration<AssetType>
    {
        public void Configure(EntityTypeBuilder<AssetType> builder)
        {
            builder.HasKey(_ => _.Id);
        }
    }
}