using VehicleRental.Domain.Abstractions;

namespace VehicleRental.Domain.Rentals.Events
{
    public sealed record RentalReservedDomainEvent(RentalId RentalId) : IDomainEvent;
}
