using Bogus;
using Dapper;
using VehicleRental.Application.Abstractions.Data;
using VehicleRental.Domain.Users;
using VehicleRental.Domain.Vehicles;
using VehicleRental.Infrastructure;
using BCryptNet = BCrypt.Net.BCrypt;

namespace VehicleRental.Api.Extensions
{
    public static class BogusExtensions
    {
        public static void SeedAuthentication(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var service = scope.ServiceProvider;
            var loggerFactory = service.GetRequiredService<ILoggerFactory>();

            try
            {
                var context = service.GetRequiredService<ApplicationDbContext>();

                if (!context.Set<User>().Any())
                {
                    var passwordHash = BCryptNet.HashPassword("Admin123$");
                    var user = User.Create(
                        new FirstName("User"),
                        new LastName("Manager"),
                        new Email("user@rental.com"),
                        new Password(passwordHash));

                    context.Add(user);

                    user = User.Create(
                        new FirstName("Admin"),
                        new LastName("Manager"),
                        new Email("admin@rental.com"),
                        new Password(passwordHash));

                    context.Add(user);

                    context.SaveChangesAsync().Wait();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<ApplicationDbContext>();
                logger.LogError(ex.Message);
            }
        }

        public static void SeedVehicles(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
            using var connection = sqlConnectionFactory.CreateConnection();

            var faker = new Faker();

            List<object> vehicles = new();
            for (var i = 0; i < 100; i++)
            {
                vehicles.Add(new
                {
                    Id = Guid.NewGuid(),
                    Vin = faker.Vehicle.Vin(),
                    Model = faker.Vehicle.Model(),
                    Country = faker.Address.Country(),
                    Department = faker.Address.State(),
                    ZipCode = faker.Address.ZipCode(),
                    City = faker.Address.City(),
                    Street = faker.Address.StreetAddress(),
                    PriceAmount = faker.Random.Decimal(50, 1000),
                    PriceCurrency = "USD",
                    MaintenanceFeeAmount = faker.Random.Decimal(25, 200),
                    MaintenanceFeeCurrency = "USD",
                    Amenities = new List<int> { (int)Amenity.Wifi, (int)Amenity.AndroidSystem },
                    LastRentedOnUtc = DateTime.MinValue
                });
            }

            const string sql = """
            INSERT INTO public.vehicles
            (id, vin, model, address_country, address_department, address_zip_code, address_city, address_street, price_amount, price_currency, maintenance_fee_amount, maintenance_fee_currency, amenities, last_rented_on_utc)
            VALUES(@Id, @Vin, @Model, @Country, @Department, @ZipCode, @City, @Street, @PriceAmount, @PriceCurrency, @MaintenanceFeeAmount, @MaintenanceFeeCurrency, @Amenities, @LastRentedOnUtc);
            """;

            connection.Execute(sql, vehicles);
        }
    }
}
