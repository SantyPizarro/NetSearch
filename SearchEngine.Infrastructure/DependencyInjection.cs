using Microsoft.Extensions.DependencyInjection;
using SearchEngine.Application.Abstractions.Services;
using SearchEngine.Infrastructure.Search;

namespace SearchEngine.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddScoped<ITokenizer, Tokenizer>();
        services.AddScoped<IRankingStrategy, TfIdfRankingStrategy>();

        return services;
    }
}
