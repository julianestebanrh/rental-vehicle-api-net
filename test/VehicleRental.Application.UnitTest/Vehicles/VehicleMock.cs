using VehicleRental.Domain.Shared;
using VehicleRental.Domain.Vehicles;

namespace VehicleRental.Application.UnitTest.Vehicles
{
    internal static class VehicleMock
    {
        public static Vehicle Create() => new(
             VehicleId.New(),
             new Model("Civic"),
             new Vin("ED984DD556"),
             new Address("Country", "State", "ZipCode", "City", "Street"),
             new Money(100.0m, Currency.USD),
             Money.Zero(),
             [],
             DateTime.UtcNow.AddYears(-1));
    }
}
