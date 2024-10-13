using Microsoft.EntityFrameworkCore;
using VehicleRental.Application.Abstractions.Pagination;
using VehicleRental.Domain.Users;

namespace VehicleRental.Infrastructure.Repositories
{
    internal sealed class UserRepository : Repository<User, UserId>, IUserRepository, IPaginationRepository<User, UserId>
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public override void Add(User user)
        {
            foreach (var role in user.Roles)
            {
                DbContext.Attach(role);
            }

            DbContext.Add(user);
        }

        public async Task<bool> DoesTheUserExist(Domain.Users.Email email, CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<User>().AnyAsync(x => x.Email == email, cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(Domain.Users.Email email, CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<User>().FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        }
    }
}
