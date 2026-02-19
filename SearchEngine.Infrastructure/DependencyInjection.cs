using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SearchEngine.Application.Abstractions.Persistence;
using SearchEngine.Application.Abstractions.Services;
using SearchEngine.Infrastructure.Persistence;
using SearchEngine.Infrastructure.Persistence.Repositories;
using SearchEngine.Infrastructure.Search;

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

        services.AddScoped<ITokenizer, Tokenizer>();
        services.AddScoped<IRankingStrategy, TfIdfRankingStrategy>();
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<ITermRepository, TermRepository>();
        services.AddScoped<IIndexRepository, IndexRepository>();

        return services;
    }
}
