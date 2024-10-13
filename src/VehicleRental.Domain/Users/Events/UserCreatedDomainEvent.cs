using VehicleRental.Domain.Abstractions;

namespace VehicleRental.Domain.Users.Events
{
    public sealed record UserCreatedDomainEvent(UserId UserId) : IDomainEvent;
}
