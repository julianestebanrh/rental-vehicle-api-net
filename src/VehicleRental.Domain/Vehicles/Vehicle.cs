using VehicleRental.Domain.Abstractions;
using VehicleRental.Domain.Shared;

namespace VehicleRental.Domain.Vehicles
{
    public sealed class Vehicle : Entity<VehicleId>
    {

        private Vehicle() { }
        public Vehicle(
            VehicleId id,
            Model model,
            Vin vin,
            Address? address,
            Money price,
            Money maintenanceFee,
            List<Amenity> amenities,
            DateTime? lastRentedOnUtc) : base(id)
        {
            Model = model;
            Vin = vin;
            Address = address;
            Price = price;
            MaintenanceFee = maintenanceFee;
            Amenities = amenities;
            LastRentedOnUtc = lastRentedOnUtc;
        }

        public Model? Model { get; private set; }
        public Vin? Vin { get; private set; }
        public Address? Address { get; private set; }
        public Money? Price { get; private set; }
        public Money? MaintenanceFee { get; private set; }
        public List<Amenity> Amenities { get; private set; } = new();
        public DateTime? LastRentedOnUtc { get; internal set; }
    }
}
