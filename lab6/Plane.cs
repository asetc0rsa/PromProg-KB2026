public abstract class Plane
{
    public string name;
    public int flightRange;
    public double fuelConsumption;

    public Plane(string name, int flightRange, double fuelConsumption)
    {
        this.name = name;
        this.flightRange = flightRange;
        this.fuelConsumption = fuelConsumption;
    }

    public abstract string GetInfo();
}
