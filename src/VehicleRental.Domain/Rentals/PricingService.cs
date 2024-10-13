using VehicleRental.Domain.Shared;
using VehicleRental.Domain.Vehicles;

namespace VehicleRental.Domain.Rentals
{
    public class PricingService
    {
        public PricingDetail CalculatePrice(Vehicle vehicle, DateRange period)
        {
            var currency = vehicle.Price!.Currency;

            var priceForPeriod = new Money(
                 vehicle.Price.Amount * period.LengthInDays,
                 currency);

            decimal percentageUpCharge = 0;
            foreach (var amenity in vehicle.Amenities)
            {
                percentageUpCharge += amenity switch
                {
                    Amenity.AppleSystem or Amenity.AndroidSystem => 0.05m,
                    Amenity.AirConditioning => 0.01m,
                    Amenity.Maps => 0.01m,
                    Amenity.Wifi => 0.01m,
                    _ => 0
                };
            }

            var amenitiesUpCharge = Money.Zero(currency);
            if (percentageUpCharge > 0)
            {
                amenitiesUpCharge = new Money(
                    priceForPeriod.Amount * percentageUpCharge,
                    currency);
            }

            var totalPrice = Money.Zero(currency);

            totalPrice += priceForPeriod;

            if (!vehicle.MaintenanceFee!.IsZero())
            {
                totalPrice += vehicle.MaintenanceFee;
            }

            totalPrice += amenitiesUpCharge;

            return new PricingDetail(priceForPeriod, vehicle.MaintenanceFee, amenitiesUpCharge, totalPrice);

        }
    }
}
