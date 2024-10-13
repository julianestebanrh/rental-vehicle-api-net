using VehicleRental.Application.Abstractions.Messaging;
using VehicleRental.Application.Abstractions.Pagination;
using VehicleRental.Domain.Shared;

namespace VehicleRental.Application.Vehicles.GetAllVehicles
{
    public sealed record GetAllVehiclesQuery : PaginationParams, IQuery<PagedResult<GetAllVehicleQueryDto>>
    {
    }
}
