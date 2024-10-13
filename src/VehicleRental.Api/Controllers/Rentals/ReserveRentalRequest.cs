namespace VehicleRental.Api.Controllers.Rentals
{
    public sealed record ReserveRentalRequest(
    Guid VehicleId,
    Guid UserId,
    DateOnly StartDate,
    DateOnly EndDate);
}
