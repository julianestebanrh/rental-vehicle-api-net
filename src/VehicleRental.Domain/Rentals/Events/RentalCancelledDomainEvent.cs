using VehicleRental.Domain.Abstractions;

namespace VehicleRental.Domain.Rentals.Events
{
    public sealed record RentalCancelledDomainEvent(RentalId RentalId) : IDomainEvent;
}
