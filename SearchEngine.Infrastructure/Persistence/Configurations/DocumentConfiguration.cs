using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SearchEngine.Domain.Documents;

public sealed class DocumentConfiguration
    : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasConversion(
                id => id.Value,
                value => DocumentId.From(value))
            .ValueGeneratedNever();

        builder.Property(d => d.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.Content)
            .IsRequired();

        builder.Property(d => d.CreatedAt);
        builder.Property(d => d.UpdatedAt);

        builder.OwnsOne(d => d.Metadata, metadataBuilder =>
        {
            metadataBuilder.Property(m => m.Author)
                .HasMaxLength(200);

            metadataBuilder.Property(m => m.Category)
                .HasMaxLength(200);
        });

        builder.OwnsMany<Tag>("_tags", tagBuilder =>
        {
            tagBuilder.ToTable("DocumentTags");

            tagBuilder.WithOwner()
                .HasForeignKey("DocumentId");

            tagBuilder.Property<int>("Id");
            tagBuilder.HasKey("Id");

            tagBuilder.Property(t => t.Value)
                .HasColumnName("Value")
                .IsRequired();
        });

        builder.Navigation("_tags")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}