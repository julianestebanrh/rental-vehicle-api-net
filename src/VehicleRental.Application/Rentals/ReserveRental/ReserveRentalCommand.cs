using VehicleRental.Application.Abstractions.Messaging;

namespace VehicleRental.Application.Rentals.ReserveRental
{
    public record ReserveRentalCommand(
        Guid VehicleId,
        Guid UserId,
        DateOnly StartDate,
        DateOnly EndDate) : ICommand<Guid>;
}
