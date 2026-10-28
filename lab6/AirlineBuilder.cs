using System;
using System.IO;

public class AirlineBuilder
{
    public Airline BuildFromFile(string fileName)
    {
        string[] lines = File.ReadAllLines(fileName);

        if (lines.Length == 0 || string.IsNullOrWhiteSpace(lines[0]))
        {
            throw new Exception("Файл пустой или не содержит название авиакомпании");
        }

        Airline airline = new Airline(lines[0].Trim());

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (line.Length == 0) continue;

            string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 5)
            {
                Console.WriteLine("Строка " + (i + 1) + " пропущена: мало данных");
                continue;
            }

            string type = parts[0];
            string name = parts[1];
            int range = int.Parse(parts[2]);
            double fuel = double.Parse(parts[3]);

            if (type == "pass")
            {
                int capacity = int.Parse(parts[4]);
                airline.AddPlane(new PassengerPlane(name, range, fuel, capacity));
            }
            else if (type == "cargo")
            {
                double capacity = double.Parse(parts[4]);
                airline.AddPlane(new CargoPlane(name, range, fuel, capacity));
            }
            else
            {
                Console.WriteLine("Строка " + (i + 1) + " пропущена: неизвестный тип " + type);
            }
        }

        return airline;
    }
}
