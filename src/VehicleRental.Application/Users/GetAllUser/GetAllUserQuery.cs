using VehicleRental.Application.Abstractions.Messaging;
using VehicleRental.Application.Abstractions.Pagination;
using VehicleRental.Domain.Shared;

namespace VehicleRental.Application.Users.GetAllUser
{
    public sealed record GetAllUserQuery : PaginationParams, IQuery<PagedResult<GetAllUserQueryDto>>
    {
    }
}
