using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleRental.Domain.Users;

namespace VehicleRental.Infrastructure.Configurations
{
    internal sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("users_roles");

            builder.HasKey(x => new { x.RoleId, x.UserId });

            builder.Property(userRole => userRole.UserId)
                .HasConversion(userId => userId!.Value, value => new UserId(value));
        }
    }
}
