using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

[XmlRoot("text")]
public class Text
{
    [XmlElement("sentence")]
    public List<Sentence> Sentences { get; set; } = new List<Sentence>();
}

public class Sentence
{
    [XmlElement("word", typeof(Word))]
    [XmlElement("punctuation", typeof(Punctuation))]
    public List<Token> Tokens { get; set; } = new List<Token>();

    [XmlIgnore]
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
    [XmlText]
    public string Value { get; set; } = string.Empty;
}

public class Word : Token { }
public class Punctuation : Token { }