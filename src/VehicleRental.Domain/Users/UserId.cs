namespace VehicleRental.Domain.Users
{
    public record UserId(Guid Value)
    {
        public static UserId New() => new(Guid.NewGuid());
    }
}
