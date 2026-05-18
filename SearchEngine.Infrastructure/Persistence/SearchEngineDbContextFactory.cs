using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SearchEngine.Infrastructure.Persistence;

public sealed class SearchEngineDbContextFactory
    : IDesignTimeDbContextFactory<SearchEngineDbContext>
{
    public SearchEngineDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        var appSettingsPath = File.Exists(Path.Combine(basePath, "appsettings.json"))
            ? basePath
            : Path.Combine(basePath, "SearchEngine.Api");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(appSettingsPath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = configuration
            .GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<SearchEngineDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new SearchEngineDbContext(optionsBuilder.Options, NoOpMediator.Instance);
    }

    private sealed class NoOpMediator : IMediator
    {
        public static readonly NoOpMediator Instance = new();

        public Task<TResponse> Send<TResponse>(
            IRequest<TResponse> request,
            CancellationToken cancellationToken = default)
        {
            return Task.FromException<TResponse>(
                new NotSupportedException("Design-time mediator cannot send requests."));
        }

        public Task Send<TRequest>(
            TRequest request,
            CancellationToken cancellationToken = default)
            where TRequest : IRequest
        {
            return Task.FromException(
                new NotSupportedException("Design-time mediator cannot send requests."));
        }

        public Task<object?> Send(
            object request,
            CancellationToken cancellationToken = default)
        {
            return Task.FromException<object?>(
                new NotSupportedException("Design-time mediator cannot send requests."));
        }

        public Task Publish<TNotification>(
            TNotification notification,
            CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            return Task.CompletedTask;
        }

        public Task Publish(
            object notification,
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(
            IStreamRequest<TResponse> request,
            CancellationToken cancellationToken = default)
        {
            return EmptyAsync<TResponse>();
        }

        public IAsyncEnumerable<object?> CreateStream(
            object request,
            CancellationToken cancellationToken = default)
        {
            return EmptyAsync<object?>();
        }

        private static async IAsyncEnumerable<T> EmptyAsync<T>()
        {
            await Task.CompletedTask;
            yield break;
        }
    }
}
