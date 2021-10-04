using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NomaNova.Ojeda.Core.Domain.FieldSets;

namespace NomaNova.Ojeda.Data.Context.Configurations
{
    public class FieldSetEntityTypeConfiguration : IEntityTypeConfiguration<FieldSet>
    {
        public void Configure(EntityTypeBuilder<FieldSet> builder)
        {
            builder.HasKey(_ => _.Id);
        }
    }
}