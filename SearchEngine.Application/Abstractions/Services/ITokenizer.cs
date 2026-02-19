namespace SearchEngine.Application.Abstractions.Services;

public interface ITokenizer
{
    IReadOnlyCollection<string> Tokenize(string text);
}
