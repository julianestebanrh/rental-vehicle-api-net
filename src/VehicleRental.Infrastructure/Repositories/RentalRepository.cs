using Microsoft.EntityFrameworkCore;
using VehicleRental.Domain.Rentals;
using VehicleRental.Domain.Vehicles;

namespace VehicleRental.Infrastructure.Repositories
{
    internal sealed class RentalRepository : Repository<Rental, RentalId>, IRentalRepository
    {

        private static readonly RentalStatus[] ActiveRentalStatuses =
           {
                RentalStatus.Reserved,
                RentalStatus.Confirmed,
                RentalStatus.Completed
            };

        public RentalRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> IsOverlappingAsync(Vehicle vehicle, DateRange duration, CancellationToken cancellationToken = default)
        {
            return await DbContext
           .Set<Rental>()
           .AnyAsync(
               rental =>
                   rental.VehicleId == vehicle.Id &&
                   rental.Duration!.Start <= duration.End &&
                   rental.Duration.End >= duration.Start &&
                   ActiveRentalStatuses.Contains(rental.Status),
               cancellationToken);
        }
    }
}
