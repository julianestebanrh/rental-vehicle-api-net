namespace VehicleRental.Application.Vehicles.GetAllVehicles
{
    public sealed record AddressDto
    {
        public string? Country { get; set; }
        public string? Department { get; set; }
        public string? ZipCode { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
    }
}
