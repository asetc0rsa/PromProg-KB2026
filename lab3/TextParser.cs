using System.Text;

public class TextParser
{
    public Text Parse(string rawText)
    {
        var text = new Text();
        var currentSentence = new Sentence();
        var currentWord = new StringBuilder();

        foreach (char c in rawText)
        {
            if (char.IsLetterOrDigit(c) || c == '-')
            {
                currentWord.Append(c);
            }
            else
            {
                if (currentWord.Length > 0)
                {
                    currentSentence.Tokens.Add(new Word { Value = currentWord.ToString() });
                    currentWord.Clear();
                }

                if (!char.IsWhiteSpace(c))
                {
                    currentSentence.Tokens.Add(new Punctuation { Value = c.ToString() });
                    
                    if (c == '.' || c == '!' || c == '?')
                    {
                        text.Sentences.Add(currentSentence);
                        currentSentence = new Sentence();
                    }
                }
            }
        }

        if (currentWord.Length > 0)
            currentSentence.Tokens.Add(new Word { Value = currentWord.ToString() });
            
        if (currentSentence.Tokens.Count > 0)
            text.Sentences.Add(currentSentence);

        return text;
    }
}