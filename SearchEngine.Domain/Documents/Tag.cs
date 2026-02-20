using SearchEngine.Domain.Common;

public sealed class Tag : ValueObject
{
    public string Value { get; private set; } = null!;

    private Tag() { }

    private Tag(string value)
    {
        Value = value;
    }

    public static Tag Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Tag cannot be empty.");

        return new Tag(value.Trim().ToLowerInvariant());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}