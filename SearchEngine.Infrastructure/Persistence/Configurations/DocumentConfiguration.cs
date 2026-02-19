using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SearchEngine.Domain.Documents;

namespace SearchEngine.Infrastructure.Persistence.Configurations;

public sealed class DocumentConfiguration
    : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable("Documents");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasConversion(
                id => id.Value,
                value => DocumentId.From(value))
            .ValueGeneratedNever();

        builder.Property(d => d.Title)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(d => d.Content)
            .IsRequired();

        builder.Property(d => d.CreatedAt)
            .IsRequired();

        builder.Property(d => d.UpdatedAt)
            .IsRequired();

        builder.OwnsOne(d => d.Metadata, metadata =>
        {
            metadata.Property(m => m.Author)
                .HasMaxLength(200)
                .HasColumnName("Author");

            metadata.Property(m => m.Category)
                .HasMaxLength(200)
                .HasColumnName("Category");
        });

        builder.OwnsMany(d => d.Tags, tags =>
        {
            tags.ToTable("DocumentTags");

            tags.WithOwner()
                .HasForeignKey("DocumentId");

            tags.Property(t => t.Value)
                .HasColumnName("Tag")
                .IsRequired()
                .HasMaxLength(100);

            tags.HasKey("DocumentId", "Tag");
        });

        builder.Navigation(d => d.Tags)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
