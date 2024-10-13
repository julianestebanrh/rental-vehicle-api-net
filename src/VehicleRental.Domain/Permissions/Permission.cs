using VehicleRental.Domain.Abstractions;

namespace VehicleRental.Domain.Permissions
{
    public sealed class Permission : Entity<PermissionId>
    {
        public Permission(PermissionId id, PermissionName? name) : base(id)
        {
            Name = name;
        }

        public Permission(PermissionName? name) : base()
        {
            Name = name;
        }

        private Permission() { }
        public PermissionName? Name { get; private set; }

        public static Result<Permission> Create(PermissionName name)
        {
            return new Permission(name);
        }
    }
}
