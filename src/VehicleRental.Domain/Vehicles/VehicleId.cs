namespace VehicleRental.Domain.Vehicles
{
    public record VehicleId(Guid Value)
    {
        public static VehicleId New() => new(Guid.NewGuid());
    }
}
