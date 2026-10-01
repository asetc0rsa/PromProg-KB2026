using System.Collections.Generic;
using System.Linq;

public class Text
{
    public List<Sentence> Sentences { get; set; } = new List<Sentence>();
}

public class Sentence
{
    public List<Token> Tokens { get; set; } = new List<Token>();

    public IEnumerable<Word> Words => Tokens.OfType<Word>();

    public override string ToString()
    {
        return string.Join(" ", Tokens.Select(t => t.Value))
                     .Replace(" ,", ",")
                     .Replace(" .", ".")
                     .Replace(" ?", "?")
                     .Replace(" !", "!");
    }
}

public abstract class Token
{
    public string Value { get; set; } = string.Empty;
}

public class Word : Token { }
public class Punctuation : Token { }