using VehicleRental.Application.Abstractions.Authentication;
using VehicleRental.Application.Abstractions.Messaging;
using VehicleRental.Domain.Abstractions;
using VehicleRental.Domain.Users;
using BCryptNet = BCrypt.Net.BCrypt;

namespace VehicleRental.Application.Users.LogInUser
{
    internal sealed class LogInUserCommandHandler : ICommandHandler<LogInUserCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public LogInUserCommandHandler(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<Result<string>> Handle(LogInUserCommand request, CancellationToken cancellationToken)
        {
            var email = new Email(request.Email);
            var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

            if (user is null)
            {
                return Result.Failure<string>(UserErrors.NotFound);
            }

            if (!BCryptNet.Verify(request.Password, user.Password!.Value))
            {
                return Result.Failure<string>(UserErrors.InvalidCredentials);
            }

            var token = await _jwtService.GetAccessTokenAsync(user);

            return token;
        }
    }
}
