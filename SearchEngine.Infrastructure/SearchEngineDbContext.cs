using Microsoft.EntityFrameworkCore;
using SearchEngine.Application.Abstractions.Persistence;
using SearchEngine.Domain.Documents;
using SearchEngine.Domain.Indexing;

namespace SearchEngine.Infrastructure.Persistence;

public sealed class SearchEngineDbContext
    : DbContext, IUnitOfWork
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

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
