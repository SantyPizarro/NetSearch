using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SearchEngine.Infrastructure.Persistence;

public sealed class SearchEngineDbContextFactory
    : IDesignTimeDbContextFactory<SearchEngineDbContext>
{
    public SearchEngineDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var connectionString = configuration
            .GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<SearchEngineDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        var services = new ServiceCollection();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateDocumentCommand).Assembly));

        var provider = services.BuildServiceProvider();
        var mediator = provider.GetService<IMediator>();

        return new SearchEngineDbContext(optionsBuilder.Options, mediator);
    }
}