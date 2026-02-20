using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.IO;

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

        return new SearchEngineDbContext(optionsBuilder.Options);
    }
}