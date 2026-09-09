using System;
using System.Collections.Generic;
using System.IO;

namespace GeneticSearch
{
    struct Protein
    {
        public string name;
        public string organism;
        public string amino_acids;
    }

    struct Command
    {
        public string name;
        public string parameter1;
        public string parameter2;
    }

    class Program
    {
        static List<Command> ReadCommands(string filename)
        {
            StreamReader reader = new StreamReader(filename);
            List<Command> commands = new List<Command>();
            Command command = new Command { name = String.Empty, parameter1 = String.Empty, parameter2 = String.Empty };
            
            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine();
                if (line == null) continue;
                string[] parts = line.Split('\t');
                if (parts.Length == 2) { command.name = parts[0]; command.parameter1 = parts[1]; command.parameter2 = String.Empty; }
                else if (parts.Length >= 3) { command.name = parts[0]; command.parameter1 = parts[1]; command.parameter2 = parts[2]; }
                commands.Add(command);
            }
            reader.Close();
            return commands;
        }

        static List<Protein> ReadData(string filename)
        {
            StreamReader reader = new StreamReader(filename);
            List<Protein> data = new List<Protein>();
            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine();
                if (line == null) continue;
                string[] parts = line.Split('\t');
                if (parts.Length >= 3)
                {
                    Protein protein = new Protein { name = parts[0], organism = parts[1], amino_acids = parts[2] };
                    data.Add(protein);
                }
            }
            reader.Close();
            return data;
        }

        static string Decoding(string amino_acids)
        {
            string decoded = String.Empty;
            for (int i = 0; i < amino_acids.Length; i++)
            {
                char ch = amino_acids[i];
                if (char.IsDigit(ch))
                {
                    char letter = amino_acids[i + 1];
                    int count = ch - '0';
                    for (int j = 1; j < count; j++) decoded += letter;
                }
                else decoded += ch;
            }
            return decoded;
        }

        static void Search(List<Protein> proteins, string sequence, StreamWriter writer, int commandNumber) => throw new NotImplementedException();
        static void Diff(List<Protein> proteins, string protein1Name, string protein2Name, StreamWriter writer, int commandNumber) => throw new NotImplementedException();
        static void Mode(List<Protein> proteins, string proteinName, StreamWriter writer, int commandNumber) => throw new NotImplementedException();
        static void CommandHandler(List<Protein> proteins, List<Command> commands, string outputFile) => throw new NotImplementedException();

        static void Main(string[] args)
        {
            Console.WriteLine("=== Genetic Search Program ===");
            Console.Write("Введите номер набора файлов (1-3, по умолчанию 1): ");
            string? choice = Console.ReadLine();
            int fileNumber = (choice != null && int.TryParse(choice, out int num) && num >= 1 && num <= 3) ? num - 1 : 0;
            
            string sequencesFile = $"sequences.{fileNumber}.txt";
            string commandsFile = $"commands.{fileNumber}.txt";
            
            try
            {
                List<Protein> data = ReadData(sequencesFile);
                Console.WriteLine($"✓ Загружено белков: {data.Count}");
                List<Command> commands = ReadCommands(commandsFile);
                Console.WriteLine($"✓ Загружено команд: {commands.Count}");
                Console.WriteLine("\n[СТАТУС] Данные успешно загружены и декодер готов. Алгоритмы поиска будут добавлены далее.");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"\n✗ Ошибка: файл не найден - {ex.FileName}");
            }
            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}