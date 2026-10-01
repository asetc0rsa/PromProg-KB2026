using System;
using System.Collections.Generic;
using System.Linq;

public class TextProcessor
{
    public void PrintSentencesByWordCount(Text text)
    {
        var sorted = text.Sentences.OrderBy(s => s.Words.Count());
        foreach (var s in sorted)
            Console.WriteLine($"[{s.Words.Count()} слов]: {s}");
    }

    public void PrintSentencesByLength(Text text)
    {
        var sorted = text.Sentences.OrderBy(s => s.Tokens.Sum(t => t.Value.Length));
        foreach (var s in sorted)
            Console.WriteLine($"[{s.Tokens.Sum(t => t.Value.Length)} симв.]: {s}");
    }

    public void FindWordsInInterrogativeSentences(Text text, int length)
    {
        var words = text.Sentences
            .Where(s => s.Tokens.LastOrDefault() is Punctuation p && p.Value.Contains("?"))
            .SelectMany(s => s.Words)
            .Where(w => w.Value.Length == length)
            .Select(w => w.Value.ToLower())
            .Distinct();

        Console.WriteLine(string.Join(", ", words));
    }

    public void RemoveConsonantWords(Text text, int length)
    {
        string consonants = "bcdfghjklmnpqrstvwxyzбвгджзйклмнпрстфхцчшщ";
        foreach (var sentence in text.Sentences)
        {
            sentence.Tokens.RemoveAll(t => 
                t is Word w && 
                w.Value.Length == length && 
                consonants.Contains(char.ToLower(w.Value[0])));
        }
    }

    public void ReplaceWordsInSentence(Text text, int sentenceIndex, int length, string substring)
    {
        if (sentenceIndex < 0 || sentenceIndex >= text.Sentences.Count) return;

        var sentence = text.Sentences[sentenceIndex];
        for (int i = 0; i < sentence.Tokens.Count; i++)
        {
            if (sentence.Tokens[i] is Word w && w.Value.Length == length)
            {
                sentence.Tokens[i] = new Word { Value = substring };
            }
        }
    }
}