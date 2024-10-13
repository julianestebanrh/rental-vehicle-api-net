using QuestPDF.Fluent;
using VehicleRental.Application.Abstractions.Messaging;

namespace VehicleRental.Application.Vehicles.ReportVehicle
{
    public sealed record ReportVehicleQuery(string Model) : IQuery<Document>;

}
