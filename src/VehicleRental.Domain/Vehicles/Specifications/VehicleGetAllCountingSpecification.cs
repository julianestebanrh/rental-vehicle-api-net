using VehicleRental.Domain.Abstractions;

namespace VehicleRental.Domain.Vehicles.Specifications
{
    public class VehicleGetAllCountingSpecification : BaseSpecification<Vehicle, VehicleId>
    {
        public VehicleGetAllCountingSpecification(string search) : base(x => string.IsNullOrEmpty(search) || x.Model == new Model(search)) { }
    }
}
