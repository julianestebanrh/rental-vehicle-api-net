using VehicleRental.Application.Abstractions.Messaging;

namespace VehicleRental.Application.Vehicles.SearchVehicles
{
    public sealed record SearchVehiclesQuery(
        DateOnly StartDate,
        DateOnly EndDate) : IQuery<IReadOnlyList<VehicleResponse>>;
}
