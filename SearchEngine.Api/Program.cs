using SearchEngine.Api.Middlewares;
using SearchEngine.Application.Abstractions.Services;
using SearchEngine.Application.Documents.Commands;
using SearchEngine.Infrastructure;
using SearchEngine.Infrastructure.Search;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SearchEngine API",
        Version = "v1",
        Description = "API para crear, indexar y buscar documentos con filtros por autor/categoria."
    });
});

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ITokenizer, Tokenizer>();
builder.Services.AddScoped<IRankingStrategy, TfIdfRankingStrategy>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateDocumentCommand).Assembly));

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
