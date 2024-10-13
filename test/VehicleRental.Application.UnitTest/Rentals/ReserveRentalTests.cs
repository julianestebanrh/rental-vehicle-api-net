using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using VehicleRental.Application.Abstractions.Clock;
using VehicleRental.Application.Exceptions;
using VehicleRental.Application.Rentals.ReserveRental;
using VehicleRental.Application.UnitTest.Users;
using VehicleRental.Application.UnitTest.Vehicles;
using VehicleRental.Domain.Abstractions;
using VehicleRental.Domain.Rentals;
using VehicleRental.Domain.Users;
using VehicleRental.Domain.Vehicles;
using Xunit;

namespace VehicleRental.Application.UnitTest.Rentals
{
    public class ReserveRentalTests
    {
        private static readonly DateTime UtcNow = DateTime.UtcNow;

        private static readonly ReserveRentalCommand Command = new(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new DateOnly(2024, 1, 1),
            new DateOnly(2024, 1, 10));

        private readonly ReserveRentalCommandHandler _handler;
        private readonly IUserRepository _userRepositoryMock;
        private readonly IVehicleRepository _vehicleRepositoryMock;
        private readonly IRentalRepository _rentalRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public ReserveRentalTests()
        {
            _userRepositoryMock = Substitute.For<IUserRepository>();
            _vehicleRepositoryMock = Substitute.For<IVehicleRepository>();
            _rentalRepositoryMock = Substitute.For<IRentalRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            IDateTimeProvider dateTimeProviderMock = Substitute.For<IDateTimeProvider>();
            dateTimeProviderMock.UtcNow.Returns(UtcNow);

            _handler = new ReserveRentalCommandHandler(
                _userRepositoryMock,
                _vehicleRepositoryMock,
                _rentalRepositoryMock,
                new PricingService(),
                 dateTimeProviderMock,
                _unitOfWorkMock
               );
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenUserIsNull()
        {
            // Arrange
            _userRepositoryMock
                .GetByIdAsync(new UserId(Command.UserId), Arg.Any<CancellationToken>())
                .Returns((User?)null);

            // Act
            var result = await _handler.Handle(Command, default);

            // Assert
            result.Error.Should().Be(UserErrors.NotFound);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenVehicleIsNull()
        {
            // Arrange
            var user = UserMock.Create();

            _userRepositoryMock
                .GetByIdAsync(new UserId(Command.UserId), Arg.Any<CancellationToken>())
                .Returns(user);

            _vehicleRepositoryMock
                .GetByIdAsync(new VehicleId(Command.VehicleId), Arg.Any<CancellationToken>())
                .Returns((Vehicle?)null);

            // Act
            var result = await _handler.Handle(Command, default);

            // Assert
            result.Error.Should().Be(VehicleErrors.NotFound);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenVehicleIsRental()
        {
            // Arrange
            var user = UserMock.Create();
            var vehicle = VehicleMock.Create();
            var duration = DateRange.Create(Command.StartDate, Command.EndDate);

            _userRepositoryMock
                .GetByIdAsync(new UserId(Command.UserId), Arg.Any<CancellationToken>())
                .Returns(user);

            _vehicleRepositoryMock
                .GetByIdAsync(new VehicleId(Command.VehicleId), Arg.Any<CancellationToken>())
                .Returns(vehicle);

            _rentalRepositoryMock
                .IsOverlappingAsync(vehicle, duration, Arg.Any<CancellationToken>())
                .Returns(true);

            // Act
            var result = await _handler.Handle(Command, default);

            // Assert
            result.Error.Should().Be(RentalErrors.Overlap);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenUnitOfWorkThrows()
        {
            // Arrange
            var user = UserMock.Create();
            var vehicle = VehicleMock.Create();
            var duration = DateRange.Create(Command.StartDate, Command.EndDate);

            _userRepositoryMock
                .GetByIdAsync(new UserId(Command.UserId), Arg.Any<CancellationToken>())
                .Returns(user);

            _vehicleRepositoryMock
                .GetByIdAsync(new VehicleId(Command.VehicleId), Arg.Any<CancellationToken>())
                .Returns(vehicle);

            _rentalRepositoryMock
                .IsOverlappingAsync(vehicle, duration, Arg.Any<CancellationToken>())
                .Returns(false);

            _unitOfWorkMock
                .SaveChangesAsync()
                .ThrowsAsync(new ConcurrencyException("Concurrency", new Exception()));

            // Act
            var result = await _handler.Handle(Command, default);

            // Assert
            result.Error.Should().Be(RentalErrors.Overlap);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenRentalIsReserved()
        {
            // Arrange
            var user = UserMock.Create();
            var vehicle = VehicleMock.Create();
            var duration = DateRange.Create(Command.StartDate, Command.EndDate);

            _userRepositoryMock
                .GetByIdAsync(new UserId(Command.UserId), Arg.Any<CancellationToken>())
                .Returns(user);

            _vehicleRepositoryMock
                .GetByIdAsync(new VehicleId(Command.VehicleId), Arg.Any<CancellationToken>())
                .Returns(vehicle);

            _rentalRepositoryMock
                .IsOverlappingAsync(vehicle, duration, Arg.Any<CancellationToken>())
                .Returns(false);

            // Act
            var result = await _handler.Handle(Command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_CallRepository_WhenRentalIsReserved()
        {
            // Arrange
            var user = UserMock.Create();
            var vehicle = VehicleMock.Create();
            var duration = DateRange.Create(Command.StartDate, Command.EndDate);

            _userRepositoryMock
                .GetByIdAsync(new UserId(Command.UserId), Arg.Any<CancellationToken>())
                .Returns(user);

            _vehicleRepositoryMock
                .GetByIdAsync(new VehicleId(Command.VehicleId), Arg.Any<CancellationToken>())
                .Returns(vehicle);

            _rentalRepositoryMock
                .IsOverlappingAsync(vehicle, duration, Arg.Any<CancellationToken>())
                .Returns(false);

            // Act
            var result = await _handler.Handle(Command, default);

            // Assert
            _rentalRepositoryMock.Received(1).Add(Arg.Is<Rental>(b => b.Id.Value == result.Value));
        }

    }
}
