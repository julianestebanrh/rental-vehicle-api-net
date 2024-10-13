using VehicleRental.Application.Abstractions.Messaging;

namespace VehicleRental.Application.Users.RegisterUser
{
    public sealed record RegisterUserCommand(string Email, string FirstName, string LastName, string Password) : ICommand<Guid>;
}
