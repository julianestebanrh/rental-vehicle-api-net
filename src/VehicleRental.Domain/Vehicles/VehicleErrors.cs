using VehicleRental.Domain.Abstractions;

namespace VehicleRental.Domain.Vehicles
{
    public static class VehicleErrors
    {
        public static Error NotFound = new(
            "Apartment.NotFound",
            "The apartment with the specified identifier was not found");
    }
}
