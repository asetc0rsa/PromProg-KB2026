using System.Text;

public class TextParser
{
    public Text Parse(string rawText)
    {
        var text = new Text();
        var currentSentence = new Sentence();
        var currentWord = new StringBuilder();

        int currentLine = 1;

        for (int i = 0; i < rawText.Length; i++)
        {
            char c = rawText[i];

            if (c == '\r')
                continue;

            if (c == '\n')
            {
                if (currentWord.Length > 0)
                {
                    currentSentence.Tokens.Add(new Word { Value = currentWord.ToString(), LineNumber = currentLine });
                    currentWord.Clear();
                }
                currentLine++;
                continue;
            }

            if (char.IsLetterOrDigit(c) || c == '-')
            {
                currentWord.Append(c);
            }
            else
            {
                if (currentWord.Length > 0)
                {
                    currentSentence.Tokens.Add(new Word { Value = currentWord.ToString(), LineNumber = currentLine });
                    currentWord.Clear();
                }

                if (!char.IsWhiteSpace(c))
                {
                    currentSentence.Tokens.Add(new Punctuation { Value = c.ToString(), LineNumber = currentLine });
                    
                    if (c == '.' || c == '!' || c == '?')
                    {
                        text.Sentences.Add(currentSentence);
                        currentSentence = new Sentence();
                    }
                }
            }
        }

        if (currentWord.Length > 0)
            currentSentence.Tokens.Add(new Word { Value = currentWord.ToString(), LineNumber = currentLine });
            
        if (currentSentence.Tokens.Count > 0)
            text.Sentences.Add(currentSentence);

        return text;
    }
}