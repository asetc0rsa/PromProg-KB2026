public class PassengerPlane : Plane
{
    public int passengerCapacity;

    public PassengerPlane(string name, int flightRange, double fuelConsumption, int passengerCapacity)
        : base(name, flightRange, fuelConsumption)
    {
        this.passengerCapacity = passengerCapacity;
    }

    public override string GetInfo()
    {
        return "Пассажирский " + name + ": дальность " + flightRange + " км, расход " + fuelConsumption + " кг/ч, мест " + passengerCapacity;
    }
}
