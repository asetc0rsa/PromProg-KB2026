using System.Collections.Generic;

public class Airline
{
    public string name;
    public List<Plane> planes;

    public Airline(string name)
    {
        this.name = name;
        planes = new List<Plane>();
    }

    public void AddPlane(Plane plane)
    {
        planes.Add(plane);
    }

    public int GetTotalCapacity()
    {
        int total = 0;
        foreach (Plane plane in planes)
        {
            if (plane is PassengerPlane passengerPlane)
            {
                total += passengerPlane.passengerCapacity;
            }
        }
        return total;
    }

    public double GetTotalLoadCapacity()
    {
        double total = 0;
        foreach (Plane plane in planes)
        {
            if (plane is CargoPlane cargoPlane)
            {
                total += cargoPlane.cargoCapacity;
            }
        }
        return total;
    }

    public List<Plane> GetPlanesByFlightRange()
    {
        List<Plane> sorted = new List<Plane>(planes);
        sorted.Sort((x, y) => x.flightRange.CompareTo(y.flightRange));
        return sorted;
    }

    public List<Plane> FindByFuelConsumption(double min, double max)
    {
        List<Plane> result = new List<Plane>();
        foreach (Plane plane in planes)
        {
            if (plane.fuelConsumption >= min && plane.fuelConsumption <= max)
            {
                result.Add(plane);
            }
        }
        return result;
    }
}
