using System.Text.RegularExpressions;
using SearchEngine.Application.Abstractions.Services;

namespace SearchEngine.Infrastructure.Search;

public sealed class Tokenizer : ITokenizer
{
    private static readonly Regex _nonWordRegex =
        new(@"[^\w\s]", RegexOptions.Compiled);

    private static readonly HashSet<string> _stopWords = new(StringComparer.OrdinalIgnoreCase)
    {
        "the", "is", "in", "at", "of", "a", "an",
        "and", "or", "to", "for", "with", "on",
        "by", "this", "that", "it", "as"
    };

    public IReadOnlyCollection<string> Tokenize(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return Array.Empty<string>();

        text = text.ToLowerInvariant();

        text = _nonWordRegex.Replace(text, " ");

        var tokens = text
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Where(t => t.Length > 2)
            .Where(t => !_stopWords.Contains(t))
            .ToList();

        return tokens;
    }
}
