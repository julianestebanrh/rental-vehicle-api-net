using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleRental.Domain.Permissions;
using VehicleRental.Domain.Roles;

namespace VehicleRental.Infrastructure.Configurations
{
    internal sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("roles_permissions");

            builder.HasKey(x => new { x.RoleId, x.PermissionId });

            builder.Property(x => x.PermissionId)
                .HasConversion(permissionId => permissionId!.Value, value => new PermissionId(value));

            builder.HasData(
                Create(Role.Client, PermissionType.UserRead),
                Create(Role.Admin, PermissionType.UserWrite),
                Create(Role.Admin, PermissionType.UserUpdate),
                Create(Role.Admin, PermissionType.UserRead)
            );
        }

        private static RolePermission Create(Role role, PermissionType permissionType)
        {
            return new RolePermission
            {
                RoleId = role.Id,
                PermissionId = new PermissionId((int)permissionType),
            };
        }
    }
}
