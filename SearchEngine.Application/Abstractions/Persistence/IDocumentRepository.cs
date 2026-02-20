using SearchEngine.Domain.Documents;
using static System.Net.Mime.MediaTypeNames;


namespace SearchEngine.Application.Abstractions.Persistence;

public interface IDocumentRepository
{
    Task<Document?> GetByIdAsync(DocumentId id, CancellationToken cancellationToken = default);
    Task<List<Document>> GetByIdsAsync(
    IEnumerable<DocumentId> ids,
    CancellationToken cancellationToken = default);

    Task AddAsync(Document document, CancellationToken cancellationToken = default);

    void Update(Document document);

    void Remove(Document document);

    Task<int> CountAsync(CancellationToken cancellationToken = default);

}