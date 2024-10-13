using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleRental.Domain.Users;

namespace VehicleRental.Infrastructure.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(user => user.Id);

            builder.Property(user => user.Id)
                .HasConversion(userId => userId!.Value, value => new UserId(value));

            builder.Property(user => user.FirstName)
            .HasMaxLength(200)
            .HasConversion(firstName => firstName!.Value, value => new FirstName(value));

            builder.Property(user => user.LastName)
                .HasMaxLength(200)
                .HasConversion(firstName => firstName!.Value, value => new LastName(value));

            builder.Property(user => user.Email)
                .HasMaxLength(400)
                .HasConversion(email => email!.Value, value => new Domain.Users.Email(value));

            builder.Property(user => user.Password)
                .HasMaxLength(2000)
                .HasConversion(password => password!.Value, value => new Password(value));

            builder.HasIndex(user => user.Email).IsUnique();

            builder.HasMany(x => x.Roles)
                .WithMany()
                .UsingEntity<UserRole>();
        }
    }
}
