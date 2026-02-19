using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SearchEngine.Domain.Indexing;

namespace SearchEngine.Infrastructure.Persistence.Configurations;

public sealed class TermConfiguration
    : IEntityTypeConfiguration<Term>
{
    public void Configure(EntityTypeBuilder<Term> builder)
    {
        builder.ToTable("Terms");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasConversion(
                id => id.Value,
                value => TermId.From(value))
            .ValueGeneratedNever();

        builder.Property(t => t.Value)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.DocumentFrequency)
            .IsRequired();

        builder.HasIndex(t => t.Value)
            .IsUnique();
    }
}
