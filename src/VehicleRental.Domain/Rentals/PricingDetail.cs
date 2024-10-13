using VehicleRental.Domain.Shared;

namespace VehicleRental.Domain.Rentals
{
    public record PricingDetail(
        Money PriceForPeriod,
        Money MaintenanceFee,
        Money AmenitiesUpCharge,
        Money TotalPrice);
}
