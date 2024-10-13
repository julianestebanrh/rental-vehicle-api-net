namespace VehicleRental.Application.Vehicles.ReportVehicle
{
    public sealed class ReportVehicleResponse
    {
        public Guid Id { get; init; }
        public string? Model { get; init; }
        public string? Vin { get; init; }
        public decimal Price { get; init; }
        public string? Currency { get; init; }
    }
}
