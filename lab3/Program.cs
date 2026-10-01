using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    private static readonly TextParser Parser = new TextParser();
    private static readonly TextProcessor Processor = new TextProcessor();

    private const string InputFileName = "text.txt";

    static void Main()
    {
        if (!File.Exists(InputFileName))
        {
            Console.WriteLine($"Ошибка: Файл '{InputFileName}' не найден!");
            return;
        }

        string inputText = File.ReadAllText(InputFileName);
        Text text = Parser.Parse(inputText);

        ExecuteTask1_SortByWordCount(text);
        ExecuteTask2_SortBySentenceLength(text);
        ExecuteTask3_FindWordsInQuestions(text, 3);
        ExecuteTask4_RemoveConsonantWords(text, 4);
        ExecuteTask5_ReplaceWordsInSentence(text, 0, 5, "REPLACED");
        ExecuteTask6_RemoveStopWords(text, "stopwords_ru.txt", "stopwords_en.txt");
        ExecuteTask7_ExportToXml(text, "output.xml");
    }

    private static void ExecuteTask1_SortByWordCount(Text text)
    {
        Console.WriteLine("=== 1. Сортировка по количеству слов ===");
        Processor.PrintSentencesByWordCount(text);
    }

    private static void ExecuteTask2_SortBySentenceLength(Text text)
    {
        Console.WriteLine("\n=== 2. Сортировка по длине предложений ===");
        Processor.PrintSentencesByLength(text);
    }

    private static void ExecuteTask3_FindWordsInQuestions(Text text, int targetLength)
    {
        Console.WriteLine($"\n=== 3. Слова длины {targetLength} в вопросительных предложениях ===");
        Processor.FindWordsInInterrogativeSentences(text, targetLength);
    }

    private static void ExecuteTask4_RemoveConsonantWords(Text text, int targetLength)
    {
        Console.WriteLine($"\n=== 4. Удаление слов длины {targetLength}, начинающихся с согласной ===");
        Processor.RemoveConsonantWords(text, targetLength);
        Console.WriteLine("Результат: " + string.Join(" ", text.Sentences));
    }

    private static void ExecuteTask5_ReplaceWordsInSentence(Text text, int sentenceIndex, int targetLength, string replacement)
    {
        Console.WriteLine($"\n=== 5. Замена слов длины {targetLength} в предложении №{sentenceIndex + 1} на '{replacement}' ===");
        Processor.ReplaceWordsInSentence(text, sentenceIndex, targetLength, replacement);
        Console.WriteLine("Результат: " + string.Join(" ", text.Sentences));
    }

    private static void ExecuteTask6_RemoveStopWords(Text text, string ruStopWordsPath, string enStopWordsPath)
    {
        Console.WriteLine("\n=== 6. Удаление стоп-слов ===");
        HashSet<string> stopWords = LoadStopWords(ruStopWordsPath, enStopWordsPath);
        Processor.RemoveStopWords(text, stopWords);
        Console.WriteLine("Текст без стоп-слов: " + string.Join(" ", text.Sentences));
    }

    private static void ExecuteTask7_ExportToXml(Text text, string outputPath)
    {
        Console.WriteLine("\n=== 7. Экспорт в XML ===");
        Processor.ExportToXml(text, outputPath);
    }

    private static HashSet<string> LoadStopWords(string ruPath, string enPath)
    {
        var stopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        if (File.Exists(ruPath))
        {
            foreach (var word in File.ReadAllLines(ruPath))
                stopWords.Add(word.Trim());
        }

        if (File.Exists(enPath))
        {
            foreach (var word in File.ReadAllLines(enPath))
                stopWords.Add(word.Trim());
        }

        return stopWords;
    }
}