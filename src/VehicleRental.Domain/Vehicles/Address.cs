namespace VehicleRental.Domain.Vehicles
{
    public record Address(
        string Country,
        string Department,
        string ZipCode,
        string City,
        string Street);
}
