using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

[XmlRoot("text")]
public class Text
{
    [XmlElement("sentence")]
    public List<Sentence> Sentences { get; set; } = new List<Sentence>();

    public SortedDictionary<string, ConcordanceEntry> BuildConcordance()
    {
        var concordance = new SortedDictionary<string, ConcordanceEntry>(System.StringComparer.OrdinalIgnoreCase);

        foreach (var sentence in Sentences)
        {
            foreach (var word in sentence.Words)
            {
                string key = word.Value.ToLower();
                if (!concordance.ContainsKey(key))
                {
                    concordance[key] = new ConcordanceEntry { Word = key };
                }
                concordance[key].Count++;
                concordance[key].LineNumbers.Add(word.LineNumber);
            }
        }

        return concordance;
    }
}

public class ConcordanceEntry
{
    public string Word { get; set; } = string.Empty;
    public int Count { get; set; }
    public SortedSet<int> LineNumbers { get; set; } = new SortedSet<int>();
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

    [XmlIgnore]
    public int LineNumber { get; set; } = 1;
}

public class Word : Token { }
public class Punctuation : Token { }