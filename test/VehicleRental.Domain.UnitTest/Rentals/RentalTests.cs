using FluentAssertions;
using VehicleRental.Domain.Rentals;
using VehicleRental.Domain.Rentals.Events;
using VehicleRental.Domain.Shared;
using VehicleRental.Domain.UnitTest.Infrastructure;
using VehicleRental.Domain.UnitTest.Users;
using VehicleRental.Domain.UnitTest.Vehicles;
using VehicleRental.Domain.Users;
using Xunit;

namespace VehicleRental.Domain.UnitTest.Rentals
{
    public class RentalTests : BaseTest
    {
        [Fact]
        public void Reserve_Should_RaiseRentalReservedDomainEvent()
        {
            // Arrange
            var user = User.Create(UserMock.FirstName, UserMock.LastName, UserMock.Email, UserMock.Password);
            var price = new Money(10.0m, Currency.USD);
            var duration = DateRange.Create(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10));
            var vehicle = VechicleMock.Create(price);
            var pricingService = new PricingService();

            // Act
            var rental = Rental.Reserve(vehicle, user.Id, duration, DateTime.UtcNow, pricingService);

            // Assert
            var rentalReservedDomainEvent = AssertDomainEventWasPublished<RentalReservedDomainEvent>(rental);

            rentalReservedDomainEvent.RentalId.Should().Be(rental.Id);
        }
    }
}
