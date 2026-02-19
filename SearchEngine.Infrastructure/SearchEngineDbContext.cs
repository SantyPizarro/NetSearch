using Microsoft.EntityFrameworkCore;
using SearchEngine.Domain.Documents;
using SearchEngine.Domain.Indexing;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SearchEngine.Infrastructure.Persistence;

public sealed class SearchEngineDbContext : DbContext
{
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<Term> Terms => Set<Term>();
    public DbSet<IndexEntry> IndexEntries => Set<IndexEntry>();

    public SearchEngineDbContext(DbContextOptions<SearchEngineDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(SearchEngineDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
