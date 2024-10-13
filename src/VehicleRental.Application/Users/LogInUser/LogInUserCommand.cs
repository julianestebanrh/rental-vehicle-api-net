using VehicleRental.Application.Abstractions.Messaging;

namespace VehicleRental.Application.Users.LogInUser
{
    public sealed record LogInUserCommand(string Email, string Password) : ICommand<string>;
}
