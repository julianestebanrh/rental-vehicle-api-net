using FluentAssertions;
using VehicleRental.Domain.Rentals;
using VehicleRental.Domain.Shared;
using VehicleRental.Domain.UnitTest.Vehicles;
using Xunit;

namespace VehicleRental.Domain.UnitTest.Rentals
{
    public class PricingServiceTests
    {
        [Fact]
        public void CalculatePrice_Should_ReturnCorrectTotalPrice()
        {
            // Arrange
            var price = new Money(10.0m, Currency.USD);
            var period = DateRange.Create(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10));
            var expectedTotalPrice = new Money(price.Amount * period.LengthInDays, price.Currency);
            var vehicle = VechicleMock.Create(price);
            var pricingService = new PricingService();

            // Act
            var pricingDetails = pricingService.CalculatePrice(vehicle, period);

            // Assert
            pricingDetails.TotalPrice.Should().Be(expectedTotalPrice);
        }

        [Fact]
        public void CalculatePrice_Should_ReturnCorrectTotalPriceWithMaintenanceFee()
        {
            // Arrange
            var price = new Money(10.0m, Currency.USD);
            var priceMaintenance = new Money(5.0m, Currency.USD);
            var period = DateRange.Create(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10));
            var expectedTotalPrice = new Money((price.Amount * period.LengthInDays) + priceMaintenance.Amount, price.Currency);
            var vehicle = VechicleMock.Create(price, priceMaintenance);
            var pricingService = new PricingService();

            // Act
            var pricingDetails = pricingService.CalculatePrice(vehicle, period);

            // Assert
            pricingDetails.TotalPrice.Should().Be(expectedTotalPrice);
        }
    }
}
