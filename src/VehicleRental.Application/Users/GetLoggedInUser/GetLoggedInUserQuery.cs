using VehicleRental.Application.Abstractions.Messaging;

namespace VehicleRental.Application.Users.GetLoggedInUser
{
    public sealed record GetLoggedInUserQuery : IQuery<UserResponse>;

}
