using VehicleRental.Application.Abstractions.Clock;
using VehicleRental.Application.Abstractions.Messaging;
using VehicleRental.Application.Exceptions;
using VehicleRental.Domain.Abstractions;
using VehicleRental.Domain.Rentals;
using VehicleRental.Domain.Users;
using VehicleRental.Domain.Vehicles;

namespace VehicleRental.Application.Rentals.ReserveRental
{
    internal sealed class ReserveRentalCommandHandler : ICommandHandler<ReserveRentalCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IRentalRepository _rentalRepository;
        public readonly IUnitOfWork _unitOfWork;
        private readonly PricingService _pricingService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ReserveRentalCommandHandler(IUserRepository userRepository, IVehicleRepository vehicleRepository, IRentalRepository rentalRepository, PricingService pricingService, IDateTimeProvider dateTimeProvider, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _vehicleRepository = vehicleRepository;
            _rentalRepository = rentalRepository;
            _pricingService = pricingService;
            _dateTimeProvider = dateTimeProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(ReserveRentalCommand request, CancellationToken cancellationToken)
        {
            var userId = new UserId(request.UserId);
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                return Result.Failure<Guid>(UserErrors.NotFound);
            }

            var vehicleId = new VehicleId(request.VehicleId);
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId, cancellationToken);

            if (vehicle is null)
            {
                return Result.Failure<Guid>(VehicleErrors.NotFound);
            }

            var duration = DateRange.Create(request.StartDate, request.EndDate);

            if (await _rentalRepository.IsOverlappingAsync(vehicle, duration, cancellationToken))
            {
                return Result.Failure<Guid>(RentalErrors.Overlap);
            }

            try
            {
                var rental = Rental.Reserve(vehicle, user.Id!, duration, _dateTimeProvider.UtcNow, _pricingService);

                _rentalRepository.Add(rental);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return rental.Id!.Value;
            }
            catch (ConcurrencyException)
            {
                return Result.Failure<Guid>(RentalErrors.Overlap);
            }
        }
    }
}
