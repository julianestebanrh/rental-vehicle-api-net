using VehicleRental.Application.Abstractions.Messaging;
using VehicleRental.Domain.Abstractions;
using VehicleRental.Domain.Shared;

namespace VehicleRental.Application.Users.GetUsers
{
    public sealed record GetUserQuery : PaginationParams, IQuery<PagedResultDapper<GetUserDto>>
    {
    }
}
