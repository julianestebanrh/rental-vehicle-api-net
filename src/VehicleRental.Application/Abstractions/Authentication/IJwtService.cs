using VehicleRental.Domain.Users;

namespace VehicleRental.Application.Abstractions.Authentication
{
    public interface IJwtService
    {
        Task<string> GetAccessTokenAsync(User user);
    }
}
