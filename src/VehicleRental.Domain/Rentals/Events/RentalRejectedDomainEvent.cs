using VehicleRental.Domain.Abstractions;

namespace VehicleRental.Domain.Rentals.Events
{
    public sealed record RentalRejectedDomainEvent(RentalId RentalId) : IDomainEvent;
}
