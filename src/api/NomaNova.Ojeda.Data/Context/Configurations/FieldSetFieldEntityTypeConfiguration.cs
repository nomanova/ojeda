using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NomaNova.Ojeda.Core.Domain.FieldSets;

namespace NomaNova.Ojeda.Data.Context.Configurations
{
    public class FieldSetFieldEntityTypeConfiguration : IEntityTypeConfiguration<FieldSetField>
    {
        public void Configure(EntityTypeBuilder<FieldSetField> builder)
        {
            builder.HasKey(_ => new {_.FieldId, _.FieldSetId});

            builder.HasOne(_ => _.Field)
                .WithMany(_ => _.FieldSetFields)
                .HasForeignKey(_ => _.FieldId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(_ => _.FieldSet)
                .WithMany(_ => _.FieldSetFields)
                .HasForeignKey(_ => _.FieldSetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}