using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NomaNova.Ojeda.Core.Domain.AssetTypes;

namespace NomaNova.Ojeda.Data.Context.Configurations
{
    public class AssetTypeFieldSetEntityTypeConfiguration : IEntityTypeConfiguration<AssetTypeFieldSet>
    {
        public void Configure(EntityTypeBuilder<AssetTypeFieldSet> builder)
        {
            builder.HasKey(_ => new {_.FieldSetId, _.AssetTypeId});
            
            builder.HasOne(_ => _.FieldSet)
                .WithMany(_ => _.AssetTypeFieldSets)
                .HasForeignKey(_ => _.FieldSetId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(_ => _.AssetType)
                .WithMany(_ => _.AssetTypeFieldSets)
                .HasForeignKey(_ => _.AssetTypeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}