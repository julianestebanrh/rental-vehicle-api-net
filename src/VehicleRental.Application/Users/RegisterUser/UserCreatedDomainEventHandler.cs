using MediatR;
using VehicleRental.Application.Abstractions.Email;
using VehicleRental.Domain.Users;
using VehicleRental.Domain.Users.Events;

namespace VehicleRental.Application.Users.RegisterUser
{
    internal sealed class UserCreatedDomainEventHandler : INotificationHandler<UserCreatedDomainEvent>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public UserCreatedDomainEventHandler(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(notification.UserId, cancellationToken);

            if (user is null)
            {
                return;
            }

            await _emailService.SendAsync(
                user.Email!,
                "Vehicle Rental: account creation!",
                "Your account was created successfully");
        }
    }
}
