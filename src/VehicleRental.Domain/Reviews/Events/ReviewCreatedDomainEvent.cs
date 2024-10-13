using VehicleRental.Domain.Abstractions;

namespace VehicleRental.Domain.Reviews.Events
{
    public sealed record ReviewCreatedDomainEvent(ReviewId ReviewId) : IDomainEvent;
}
