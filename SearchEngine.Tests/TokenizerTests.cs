using SearchEngine.Infrastructure.Search;

namespace SearchEngine.Tests;

public sealed class TokenizerTests
{
    [Fact]
    public void Tokenize_NormalizesTextAndRemovesStopWordsAndShortWords()
    {
        var tokenizer = new Tokenizer();

        var tokens = tokenizer.Tokenize("The QUICK, brown fox is in AI search.");

        Assert.Equal(new[] { "quick", "brown", "fox", "search" }, tokens);
    }
}
