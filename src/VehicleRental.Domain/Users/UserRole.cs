namespace VehicleRental.Domain.Users
{
    public sealed class UserRole
    {
        public int RoleId { get; private set; }
        public UserId? UserId { get; private set; }
    }
}
