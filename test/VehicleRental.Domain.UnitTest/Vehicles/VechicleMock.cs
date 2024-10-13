using VehicleRental.Domain.Shared;
using VehicleRental.Domain.Vehicles;

namespace VehicleRental.Domain.UnitTest.Vehicles
{
    internal static class VechicleMock
    {
        public static Vehicle Create(Money price, Money? maintenanceFee = null) => new(
             VehicleId.New(),
             new Model("Civic"),
             new Vin("ED984DD556"),
             new Address("Country", "State", "ZipCode", "City", "Street"),
             price,
             maintenanceFee ?? Money.Zero(),
             [],
             DateTime.UtcNow.AddYears(-1));
    }
}
