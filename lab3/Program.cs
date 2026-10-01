using System;

class Program
{
    static void Main()
    {
        string sampleText = "Peter Piper picked a peck of pickled peppers. Did Peter Piper pick a peck of pickled peppers? Yes, he did!";

        var parser = new TextParser();
        var processor = new TextProcessor();

        Text text = parser.Parse(sampleText);

        Console.WriteLine("=== 1. Сортировка по количеству слов ===");
        processor.PrintSentencesByWordCount(text);

        Console.WriteLine("\n=== 2. Сортировка по длине предложений ===");
        processor.PrintSentencesByLength(text);

        Console.WriteLine("\n=== 3. Поиск слов длины 5 в вопросительных предложениях ===");
        processor.FindWordsInInterrogativeSentences(text, 5);

        Console.WriteLine("\n=== 4. Удаление слов длины 5, начинающихся с согласной ===");
        processor.RemoveConsonantWords(text, 5);
        Console.WriteLine(string.Join(" ", text.Sentences));

        Console.WriteLine("\n=== 5. Замена слов длины 5 в 1-м предложении ===");
        processor.ReplaceWordsInSentence(text, 0, 5, "TEST");
        Console.WriteLine(string.Join(" ", text.Sentences));
    }
}