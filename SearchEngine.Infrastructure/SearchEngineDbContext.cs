using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SearchEngine.Application.Abstractions.Persistence;
using SearchEngine.Domain.Documents;
using SearchEngine.Domain.Indexing;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SearchEngine.Infrastructure.Persistence;

public sealed class SearchEngineDbContext
    : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;

    public DbSet<Document> Documents => Set<Document>();
    public DbSet<Term> Terms => Set<Term>();
    public DbSet<IndexEntry> IndexEntries => Set<IndexEntry>();

    public SearchEngineDbContext(
        DbContextOptions<SearchEngineDbContext> options,
        IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(SearchEngineDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        await DispatchDomainEventsAsync(cancellationToken);

        return result;
    }

    private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
    {
        var domainEvents = new List<INotification>();
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is not null)
            .ToList();

        foreach (var entry in entries)
        {
            var entity = entry.Entity;
            var domainEventsProperty = entity.GetType()
                .GetProperty("DomainEvents", BindingFlags.Instance | BindingFlags.Public);

            if (domainEventsProperty is null)
                continue;

            var value = domainEventsProperty.GetValue(entity) as IEnumerable<object>;
            if (value is null)
                continue;

            foreach (var evt in value)
            {
                if (evt is INotification notification)
                    domainEvents.Add(notification);
            }

            var clearMethod = entity.GetType()
                .GetMethod("ClearDomainEvents", BindingFlags.Instance | BindingFlags.Public);
            clearMethod?.Invoke(entity, null);
        }

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }
    }
}
