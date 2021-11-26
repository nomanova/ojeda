using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NomaNova.Ojeda.Core.Domain.Assets;

namespace NomaNova.Ojeda.Data.Context.Configurations
{
    public class FieldValueEntityTypeConfiguration : IEntityTypeConfiguration<FieldValue>
    {
        public void Configure(EntityTypeBuilder<FieldValue> builder)
        {
            builder.HasKey(_ => _.Id);
            
            builder.HasOne(_ => _.Asset)
                .WithMany(_ => _.FieldValues)
                .HasForeignKey(_ => _.AssetId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(_ => _.FieldSet)
                .WithMany(_ => _.FieldValues)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(_ => _.Field)
                .WithMany(_ => _.FieldValues)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}