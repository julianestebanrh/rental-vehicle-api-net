using MediatR;
using VehicleRental.Application.Abstractions.Email;
using VehicleRental.Domain.Rentals;
using VehicleRental.Domain.Rentals.Events;
using VehicleRental.Domain.Users;

namespace VehicleRental.Application.Rentals.ReserveRental
{
    internal sealed class RentalReservedDomainEventHandler : INotificationHandler<RentalReservedDomainEvent>
    {

        private readonly IRentalRepository _rentalRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public RentalReservedDomainEventHandler(IRentalRepository rentalRepository, IUserRepository userRepository, IEmailService emailService)
        {
            _rentalRepository = rentalRepository;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task Handle(RentalReservedDomainEvent notification, CancellationToken cancellationToken)
        {
            var rental = await _rentalRepository.GetByIdAsync(notification.RentalId, cancellationToken);

            if (rental is null)
            {
                return;
            }

            var user = await _userRepository.GetByIdAsync(rental.UserId!, cancellationToken);

            if (user is null)
            {
                return;
            }

            await _emailService.SendAsync(
                user.Email!,
                "Rental reserved!",
                "You have 10 minutes to confirm this rental");
        }
    }
}
