public class CargoPlane : Plane
{
    public double cargoCapacity;

    public CargoPlane(string name, int flightRange, double fuelConsumption, double cargoCapacity)
        : base(name, flightRange, fuelConsumption)
    {
        this.cargoCapacity = cargoCapacity;
    }

    public override string GetInfo()
    {
        return "Грузовой " + name + ": дальность " + flightRange + " км, расход " + fuelConsumption + " кг/ч, грузоподъемность " + cargoCapacity + " т";
    }
}
