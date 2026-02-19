using SearchEngine.Domain.Common;

namespace SearchEngine.Domain.Indexing;

public sealed class TermId : ValueObject
{
    public Guid Value { get; }

    private TermId(Guid value)
    {
        Value = value;
    }

    public static TermId CreateUnique()
        => new(Guid.NewGuid());

    public static TermId From(Guid value)
        => new(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
