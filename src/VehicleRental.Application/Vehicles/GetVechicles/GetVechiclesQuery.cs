using VehicleRental.Application.Abstractions.Messaging;
using VehicleRental.Domain.Abstractions;
using VehicleRental.Domain.Shared;
using VehicleRental.Domain.Vehicles;

namespace VehicleRental.Application.Vehicles.GetVechicles
{
    public sealed record GetVechiclesQuery : SpecificationParams, IQuery<PagedResponse<Vehicle, VehicleId>>
    {
        public string? Model { get; init; }
    }
}
