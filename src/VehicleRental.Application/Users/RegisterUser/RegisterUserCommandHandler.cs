using VehicleRental.Application.Abstractions.Messaging;
using VehicleRental.Domain.Abstractions;
using VehicleRental.Domain.Users;
using BCryptNet = BCrypt.Net.BCrypt;

namespace VehicleRental.Application.Users.RegisterUser
{
    internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public RegisterUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var email = new Email(request.Email);
            var userExists = await _userRepository.DoesTheUserExist(email);

            if (userExists)
            {
                return Result.Failure<Guid>(UserErrors.AlreadyExists);
            }

            var passwordHash = BCryptNet.HashPassword(request.Password);

            var user = User.Create(
                    new FirstName(request.FirstName),
                    new LastName(request.LastName),
                    new Email(request.Email),
                    new Password(passwordHash)
                );

            _userRepository.Add(user);
            await _unitOfWork.SaveChangesAsync();

            return user.Id!.Value;
        }
    }
}
