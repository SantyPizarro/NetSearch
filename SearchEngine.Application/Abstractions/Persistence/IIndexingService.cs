using SearchEngine.Domain.Documents;
using System.Threading;

namespace SearchEngine.Application.Abstractions.Services;

public interface IIndexingService
{
    Task IndexAsync(Document document, CancellationToken cancellationToken = default);
}