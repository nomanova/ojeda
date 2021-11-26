using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NomaNova.Ojeda.Core.Domain.Assets;

namespace NomaNova.Ojeda.Data.Context.Configurations
{
    public class AssetEntityTypeConfiguration : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.HasKey(_ => _.Id);
        
            builder.HasOne(_ => _.AssetType)
                .WithMany(_ => _.Assets)
                .HasForeignKey(_ => _.AssetTypeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}