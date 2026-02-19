using SearchEngine.Domain.Common;

namespace SearchEngine.Domain.Documents;

public sealed class DocumentId : ValueObject
{
    public Guid Value { get; }

    private DocumentId(Guid value)
    {
        Value = value;
    }

    public static DocumentId CreateUnique()
        => new(Guid.NewGuid());

    public static DocumentId From(Guid value)
        => new(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
