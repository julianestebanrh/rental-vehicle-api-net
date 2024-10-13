namespace VehicleRental.Infrastructure.Authentication
{
    public class JwtOptions
    {
        public string? SecretKey { get; init; }
        public string? Audience { get; init; }
        public string? Issuer { get; init; }
    }
}
