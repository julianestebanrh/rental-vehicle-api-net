using VehicleRental.Application.Abstractions.Messaging;

namespace VehicleRental.Application.Rentals.GetRental
{
    public sealed record GetRentalQuery(Guid RentalId) : IQuery<RentalResponse>
    {

    }
}
