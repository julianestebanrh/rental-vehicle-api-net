using VehicleRental.Application.Abstractions.Clock;

namespace VehicleRental.Infrastructure.Clock
{
    internal sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
