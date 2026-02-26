using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SearchEngine.Application.Abstractions.Persistence;
using SearchEngine.Application.Abstractions.Services;
using SearchEngine.Infrastructure.Persistence;
using SearchEngine.Infrastructure.Persistence.Repositories;
using SearchEngine.Infrastructure.Search;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore;
using SearchEngine.Infrastructure.Indexing;
using SearchEngine.Application.Search.Handlers;

namespace SearchEngine.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<SearchEngineDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<ITermRepository, TermRepository>();
        services.AddScoped<IIndexRepository, IndexRepository>();

        services.AddScoped<IIndexingService, IndexingService>();

        services.AddScoped<ITokenizer, Tokenizer>();
        services.AddScoped<IRankingStrategy, TfIdfRankingStrategy>();

        services.AddScoped<ISearchService, SearchService>();

        services.AddTransient<SearchDocumentsHandler>();

        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<SearchEngineDbContext>());

        return services;
    }
}