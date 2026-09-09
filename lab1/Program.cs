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
        static List<Command> ReadCommands(string filename) => throw new NotImplementedException();
        static List<Protein> ReadData(string filename) => throw new NotImplementedException();
        static string Decoding(string amino_acids) => throw new NotImplementedException();
        static void Search(List<Protein> proteins, string sequence, StreamWriter writer, int commandNumber) => throw new NotImplementedException();
        static void Diff(List<Protein> proteins, string protein1Name, string protein2Name, StreamWriter writer, int commandNumber) => throw new NotImplementedException();
        static void Mode(List<Protein> proteins, string proteinName, StreamWriter writer, int commandNumber) => throw new NotImplementedException();
        static void CommandHandler(List<Protein> proteins, List<Command> commands, string outputFile) => throw new NotImplementedException();

        static void Main(string[] args)
        {
            Console.WriteLine("=== Genetic Search Program ===");
            Console.WriteLine("Выберите набор файлов:");
            Console.WriteLine("1. sequences.0.txt + commands.0.txt");
            Console.WriteLine("2. sequences.1.txt + commands.1.txt");
            Console.WriteLine("3. sequences.2.txt + commands.2.txt");
            Console.Write("\nВведите номер (1-3): ");
            
            string? choice = Console.ReadLine();
            int fileNumber = 0;
            
            if (choice != null && int.TryParse(choice, out int num) && num >= 1 && num <= 3)
                fileNumber = num - 1;
            else
                Console.WriteLine("Неверный выбор. Используется набор 0 по умолчанию.");
            
            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}