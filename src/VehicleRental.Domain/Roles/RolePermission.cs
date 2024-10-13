using VehicleRental.Domain.Permissions;

namespace VehicleRental.Domain.Roles
{
    public sealed class RolePermission
    {
        public int RoleId { get; set; }
        public PermissionId? PermissionId { get; set; }
    }
}
