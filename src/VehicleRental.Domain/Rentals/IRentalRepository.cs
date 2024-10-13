using VehicleRental.Domain.Vehicles;

namespace VehicleRental.Domain.Rentals
{
    public interface IRentalRepository
    {
        Task<Rental?> GetByIdAsync(RentalId id, CancellationToken cancellationToken = default);
        Task<bool> IsOverlappingAsync(Vehicle vehicle, DateRange duration, CancellationToken cancellationToken = default);
        void Add(Rental rental);
    }
}
