using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http.Json;
using Testcontainers.PostgreSql;
using VehicleRental.Api.FunctionalTest.Users;
using VehicleRental.Application.Abstractions.Data;
using VehicleRental.Infrastructure;
using VehicleRental.Infrastructure.Data;
using Xunit;

namespace VehicleRental.Api.FunctionalTest.Infrastructure
{
    public class FunctionalTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
           .WithImage("postgres:16.0")
           .WithDatabase("vehicle_rental")
           .WithUsername("postgres")
           .WithPassword("postgres")
           .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));

                services.AddDbContext<ApplicationDbContext>(options =>
                    options
                        .UseNpgsql(_dbContainer.GetConnectionString())
                        .UseSnakeCaseNamingConvention());

                services.RemoveAll(typeof(ISqlConnectionFactory));

                services.AddSingleton<ISqlConnectionFactory>(_ =>
                    new SqlConnectionFactory(_dbContainer.GetConnectionString()));

            });
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();

            await InitializeTestUserAsync();
        }

        public new async Task DisposeAsync()
        {
            await _dbContainer.StopAsync();
        }

        private async Task InitializeTestUserAsync()
        {
            var httpClient = CreateClient();

            await httpClient.PostAsJsonAsync("api/v1/users/register", UserMock.RegisterTestUserRequest);
        }
    }
}
