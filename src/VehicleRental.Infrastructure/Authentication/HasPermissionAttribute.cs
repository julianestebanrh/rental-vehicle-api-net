using Microsoft.AspNetCore.Authorization;
using VehicleRental.Domain.Permissions;

namespace VehicleRental.Infrastructure.Authentication
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(PermissionType permissionType) : base(policy: permissionType.ToString()) { }
    }
}
