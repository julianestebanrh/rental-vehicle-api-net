using FluentAssertions;
using VehicleRental.Application.IntegrationTest.Infrastructure;
using VehicleRental.Application.Vehicles.SearchVehicles;
using Xunit;

namespace VehicleRental.Application.IntegrationTest.Vehicles
{
    public class SearchVehiclesTests : BaseIntegrationTest
    {
        public SearchVehiclesTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task SearchVehicles_ShouldReturnEmptyList_WhenDateRangeInvalid()
        {
            // Arrange
            var query = new SearchVehiclesQuery(new DateOnly(2024, 1, 10), new DateOnly(2024, 1, 1));

            // Act
            var result = await Sender.Send(query);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEmpty();
        }

        [Fact]
        public async Task SearchVehicles_ShouldReturnVehicles_WhenDateRangeIsValid()
        {
            // Arrange
            var query = new SearchVehiclesQuery(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10));

            // Act
            var result = await Sender.Send(query);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }
    }
}
