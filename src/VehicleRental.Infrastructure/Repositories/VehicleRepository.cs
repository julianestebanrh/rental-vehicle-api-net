using VehicleRental.Application.Abstractions.Pagination;
using VehicleRental.Domain.Vehicles;

namespace VehicleRental.Infrastructure.Repositories
{
    internal sealed class VehicleRepository : Repository<Vehicle, VehicleId>, IVehicleRepository, IPaginationRepository<Vehicle, VehicleId>
    {
        public VehicleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
