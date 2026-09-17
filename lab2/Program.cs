using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Game.InputFile = "1.ChaseData.txt";
        Game.OutFile = "1.PursuitLog.txt";

        if (!File.Exists(Game.InputFile))
        {
            Console.WriteLine($"Файл {Game.InputFile} не найден.");
            return;
        }

        int fieldSize = 0;
        using (StreamReader reader = new StreamReader(Game.InputFile))
        {
            string? firstLine = reader.ReadLine();
            if (firstLine != null)
            {
                fieldSize = int.Parse(firstLine.Trim());
            }
        }

        Game game = new Game(fieldSize);
        game.Run();
        
        Console.WriteLine($"Симуляция завершена! Результат сохранен в {Game.OutFile}");
    }
}