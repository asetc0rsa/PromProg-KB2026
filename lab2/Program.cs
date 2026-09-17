using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        //Запрашиваем у пользователя выбор файлов
        SelectFiles();

        //Проверяем, существует ли выбранный файл
        if (!IsFileValid())
        {
            return; //Завершаем программу, если файла нет
        }

        //Считываем размер поля из первой строки файла
        int fieldSize = ReadFieldSize();

        //Запускаем симуляцию
        RunGame(fieldSize);
    }

    private static void SelectFiles()
    {
        string choice = "";

        while (choice != "1" && choice != "2" && choice != "3")
        {
            Console.WriteLine("Выберите файл для симуляции:");
            Console.WriteLine("1 - 1.ChaseData.txt");
            Console.WriteLine("2 - 2.ChaseData.txt");
            Console.WriteLine("3 - 3.ChaseData.txt");
            Console.Write("Ваш выбор (1, 2 или 3): ");

            choice = Console.ReadLine()?.Trim() ?? "";

            if (choice != "1" && choice != "2" && choice != "3")
            {
                Console.WriteLine("Ошибка: введите только цифру 1, 2 или 3!\n");
            }
        }

        Game.InputFile = $"{choice}.ChaseData.txt";
        Game.OutFile = $"{choice}.PursuitLog.txt";

        Console.WriteLine($"\nВыбран входной файл: {Game.InputFile}");
    }

    private static bool IsFileValid()
    {
        if (!File.Exists(Game.InputFile))
        {
            Console.WriteLine($"Файл {Game.InputFile} не найден.");
            return false;
        }
        return true;
    }

    private static int ReadFieldSize()
    {
        using (StreamReader reader = new StreamReader(Game.InputFile))
        {
            string? firstLine = reader.ReadLine();
            if (firstLine != null)
            {
                return int.Parse(firstLine.Trim());
            }
        }
        return 0;
    }

    private static void RunGame(int fieldSize)
    {
        Game game = new Game(fieldSize);
        game.Run();
        
        Console.WriteLine($"Результат сохранен в файл {Game.OutFile}");
    }
}