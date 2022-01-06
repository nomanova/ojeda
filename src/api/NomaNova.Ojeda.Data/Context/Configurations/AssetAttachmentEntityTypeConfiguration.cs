using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NomaNova.Ojeda.Core.Domain.AssetAttachments;

namespace NomaNova.Ojeda.Data.Context.Configurations;

public class AssetAttachmentEntityTypeConfiguration : IEntityTypeConfiguration<AssetAttachment>
{
    public void Configure(EntityTypeBuilder<AssetAttachment> builder)
    {
        builder.HasKey(_ => _.Id);
        
        builder.HasOne(_ => _.Asset)
            .WithMany(_ => _.Attachments)
            .HasForeignKey(_ => _.AssetId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}