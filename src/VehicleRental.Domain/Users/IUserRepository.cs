namespace VehicleRental.Domain.Users
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);
        void Add(User user);
        Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
        Task<bool> DoesTheUserExist(Email email, CancellationToken cancellationToken = default);
    }
}
