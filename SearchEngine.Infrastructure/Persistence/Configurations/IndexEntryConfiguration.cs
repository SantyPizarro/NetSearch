using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SearchEngine.Domain.Indexing;
using SearchEngine.Domain.Documents;

namespace SearchEngine.Infrastructure.Persistence.Configurations;

public sealed class IndexEntryConfiguration
    : IEntityTypeConfiguration<IndexEntry>
{
    public void Configure(EntityTypeBuilder<IndexEntry> builder)
    {
        builder.ToTable("IndexEntries");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        builder.Property(e => e.TermId)
            .HasConversion(
                id => id.Value,
                value => TermId.From(value))
            .IsRequired();

        builder.Property(e => e.DocumentId)
            .HasConversion(
                id => id.Value,
                value => DocumentId.From(value))
            .IsRequired();

        builder.Property(e => e.TermFrequency)
            .IsRequired();

        builder.HasIndex(e => new { e.TermId, e.DocumentId })
            .IsUnique();
    }
}
