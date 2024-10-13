using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleRental.Domain.Permissions;

namespace VehicleRental.Infrastructure.Configurations
{
    internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("permissions");

            builder.HasKey(permission => permission.Id);

            builder.Property(permission => permission.Id)
                .HasConversion(permissionId => permissionId!.Value, value => new PermissionId(value));

            builder.Property(permission => permission.Name)
                .HasConversion(permissionName => permissionName!.Value, value => new PermissionName(value));

            var permissions = Enum.GetValues<PermissionType>().Select(x => new Permission(new PermissionId((int)x), new PermissionName(x.ToString())));
            builder.HasData(permissions);
        }
    }
}
