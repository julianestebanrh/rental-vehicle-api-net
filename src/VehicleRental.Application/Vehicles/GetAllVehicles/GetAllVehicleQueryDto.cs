namespace VehicleRental.Application.Vehicles.GetAllVehicles
{
    public sealed record GetAllVehicleQueryDto
    {
        public string? Model { get; set; }
        public string? Vin { get; set; }
        public decimal? Price { get; set; }
        public string? PriceCurrency { get; set; }
        public decimal? MaintenanceFee { get; set; }
        public string? MaintenanceFeeCurrency { get; set; }
        public List<string> Amenities { get; set; } = new();
        public DateTime? LastRentedOnUtc { get; internal set; }
        public AddressDto? Address { get; set; }

    }

}
