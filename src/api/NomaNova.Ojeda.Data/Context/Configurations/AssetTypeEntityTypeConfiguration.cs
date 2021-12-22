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

            // Asset Id Types cannot be deleted as long as an Asset Type uses it
            // Not doing so would cascade the deletion of an Asset Id Type into all
            // Asset Types, and subsequently all Assets using those Asset Types
            builder.HasOne(_ => _.AssetIdType)
                .WithMany(_ => _.AssetTypes)
                .HasForeignKey(_ => _.AssetIdTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}