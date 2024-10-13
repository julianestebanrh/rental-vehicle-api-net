using VehicleRental.Domain.Abstractions;

namespace VehicleRental.Domain.Vehicles
{
    public interface IVehicleRepository
    {
        Task<Vehicle?> GetByIdAsync(VehicleId id, CancellationToken cancellationToken = default);
        Task<int> CountAsync(ISpecification<Vehicle, VehicleId> specification, CancellationToken cancellation = default);
        Task<IReadOnlyList<Vehicle>> GetAllAsync(ISpecification<Vehicle, VehicleId> specification, CancellationToken cancellationToken = default);
    }
}
