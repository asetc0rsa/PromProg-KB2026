using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            Command command;
            command.name = String.Empty;
            command.parameter1 = String.Empty;
            command.parameter2 = String.Empty;
            
            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine();
                if (line == null) continue;
                
                string[] parts = line.Split('\t');
                if (parts.Length == 2)
                {
                    command.name = parts[0];
                    command.parameter1 = parts[1];
                    command.parameter2 = String.Empty;
                }
                else if (parts.Length >= 3)
                {
                    command.name = parts[0];
                    command.parameter1 = parts[1];
                    command.parameter2 = parts[2];
                }
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
                    Protein protein;
                    protein.name = parts[0];
                    protein.organism = parts[1];
                    protein.amino_acids = parts[2];
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
                    for (int j = 1; j < count; j++)
                        decoded = decoded + letter;
                }
                else 
                    decoded = decoded + ch;
            }
            return decoded;
        }

        static void Search(List<Protein> proteins, string sequence, StreamWriter writer, int commandNumber)
        {
            string decodedSequence = Decoding(sequence);
            
            writer.WriteLine("{0:D3}   search   {1}", commandNumber, decodedSequence);
            writer.WriteLine("organism\t\t\tprotein");
            
            bool found = false;
            foreach (Protein protein in proteins)
            {
                if (protein.amino_acids.Contains(decodedSequence))
                {
                    writer.WriteLine("{0}\t\t{1}", protein.organism, protein.name);
                    found = true;
                }
            }
            
            if (!found)
            {
                writer.WriteLine("NOT FOUND");
            }
            writer.WriteLine("--------------------------------------------------------------------------");
        }

        static void Diff(List<Protein> proteins, string protein1Name, string protein2Name, StreamWriter writer, int commandNumber)
        {
            writer.WriteLine("{0:D3}   diff   {1}   {2}", commandNumber, protein1Name, protein2Name);
            writer.WriteLine("amino-acids difference:");
            
            Protein? protein1 = null;
            Protein? protein2 = null;
            
            foreach (Protein protein in proteins)
            {
                if (protein.name == protein1Name) 
                    protein1 = protein;
                if (protein.name == protein2Name) 
                    protein2 = protein;
            }
            
            if (protein1 == null && protein2 == null)
            {
                writer.WriteLine("MISSING: {0}, {1}", protein1Name, protein2Name);
            }
            else if (protein1 == null)
            {
                writer.WriteLine("MISSING: {0}", protein1Name);
            }
            else if (protein2 == null)
            {
                writer.WriteLine("MISSING: {0}", protein2Name);
            }
            else
            {
                string seq1 = protein1.Value.amino_acids;
                string seq2 = protein2.Value.amino_acids;
                int diff = 0;
                int maxLen = Math.Max(seq1.Length, seq2.Length);
                
                for (int i = 0; i < maxLen; i++)
                {
                    if (i >= seq1.Length || i >= seq2.Length || seq1[i] != seq2[i])
                    {
                        diff++;
                    }
                }
                writer.WriteLine(diff);
            }
            writer.WriteLine("--------------------------------------------------------------------------");
        }

        static void Mode(List<Protein> proteins, string proteinName, StreamWriter writer, int commandNumber)
        {
            writer.WriteLine("{0:D3}   mode   {1}", commandNumber, proteinName);
            writer.WriteLine("amino-acid occurs:");
            
            Protein? protein = null;
            foreach (Protein p in proteins)
            {
                if (p.name == proteinName) 
                    protein = p;
            }
            
            if (protein == null)
            {
                writer.WriteLine("MISSING: {0}", proteinName);
            }
            else
            {
                string seq = protein.Value.amino_acids;
                Dictionary<char, int> counts = new Dictionary<char, int>();
                
                foreach (char ch in seq)
                {
                    if (counts.ContainsKey(ch))
                        counts[ch]++;
                    else
                        counts[ch] = 1;
                }
                
                int maxCount = 0;
                char maxChar = ' ';
                
                foreach (var kvp in counts)
                {
                    if (kvp.Value > maxCount || (kvp.Value == maxCount && kvp.Key < maxChar))
                    {
                        maxCount = kvp.Value;
                        maxChar = kvp.Key;
                    }
                }
                writer.WriteLine("{0}          {1}", maxChar, maxCount);
            }
            writer.WriteLine("--------------------------------------------------------------------------");
        }

        static void CommandHandler(List<Protein> proteins, List<Command> commands, string outputFile)
        {
            StreamWriter writer = new StreamWriter(outputFile);
            writer.WriteLine("Dwight Barnette");
            writer.WriteLine("Genetic Searching");
            writer.WriteLine("--------------------------------------------------------------------------");
            
            for (int i = 0; i < commands.Count; i++)
            {
                int commandNumber = i + 1;
                if (commands[i].name == "search")
                {
                    Search(proteins, commands[i].parameter1, writer, commandNumber);
                }
                else if (commands[i].name == "diff")
                {
                    Diff(proteins, commands[i].parameter1, commands[i].parameter2, writer, commandNumber);
                }
                else if (commands[i].name == "mode")
                {
                    Mode(proteins, commands[i].parameter1, writer, commandNumber);
                }
            }
            writer.Close();
        }

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
            {
                fileNumber = num - 1;
            }
            else
            {
                Console.WriteLine("Неверный выбор. Используется набор 0 по умолчанию.");
                fileNumber = 0;
            }
            
            string sequencesFile = $"sequences.{fileNumber}.txt";
            string commandsFile = $"commands.{fileNumber}.txt";
            string outputFile = $"genedata.{fileNumber}.txt";
            
            Console.WriteLine($"\nИспользуемые файлы:");
            Console.WriteLine($"  Последовательности: {sequencesFile}");
            Console.WriteLine($"  Команды: {commandsFile}");
            Console.WriteLine($"  Результат: {outputFile}");
            
            try
            {
                List<Protein> data = ReadData(sequencesFile);
                Console.WriteLine($"\nЗагружено белков: {data.Count}");
                
                List<Command> commands = ReadCommands(commandsFile);
                Console.WriteLine($"Загружено команд: {commands.Count}");
                
                CommandHandler(data, commands, outputFile);
                
                Console.WriteLine($"\n✓ Результаты записаны в {outputFile}");
                Console.WriteLine("\nНажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"\n✗ Ошибка: файл не найден - {ex.FileName}");
                Console.WriteLine("Убедитесь, что файлы существуют в папке с программой.");
                Console.WriteLine("\nНажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Произошла ошибка: {ex.Message}");
                Console.WriteLine("\nНажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }
    }
}