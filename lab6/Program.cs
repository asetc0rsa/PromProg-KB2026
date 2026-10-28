using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        string fileName = "planes.txt";
        if (!File.Exists(fileName))
        {
            Console.WriteLine("Ошибка: Файл '" + fileName + "' не найден!");
            return;
        }

        Airline airline;
        try
        {
            airline = new AirlineBuilder().BuildFromFile(fileName);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка чтения файла: " + ex.Message);
            return;
        }

        Console.WriteLine("Авиакомпания \"" + airline.name + "\"");
        Console.WriteLine();
        Console.WriteLine("Самолеты компании:");
        foreach (Plane plane in airline.planes)
        {
            Console.WriteLine("  " + plane.GetInfo());
        }

        Console.WriteLine();
        Console.WriteLine("Общая вместимость: " + airline.GetTotalCapacity() + " пассажиров");
        Console.WriteLine("Общая грузоподъемность: " + airline.GetTotalLoadCapacity() + " тонн");

        Console.WriteLine();
        Console.WriteLine("Сортировка по дальности полета:");
        foreach (Plane plane in airline.GetPlanesByFlightRange())
        {
            Console.WriteLine("  " + plane.name + " - " + plane.flightRange + " км");
        }

        double min = 800;
        double max = 1000;
        Console.WriteLine();
        Console.WriteLine("Поиск по расходу топлива от " + min + " до " + max + " кг/ч:");
        List<Plane> found = airline.FindByFuelConsumption(min, max);
        if (found.Count == 0)
        {
            Console.WriteLine("  Ничего не найдено");
        }
        else
        {
            foreach (Plane plane in found)
            {
                Console.WriteLine("  " + plane.GetInfo());
            }
        }
    }
}
