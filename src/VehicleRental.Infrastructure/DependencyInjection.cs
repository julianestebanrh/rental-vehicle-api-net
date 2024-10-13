using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using VehicleRental.Application.Abstractions.Authentication;
using VehicleRental.Application.Abstractions.Clock;
using VehicleRental.Application.Abstractions.Data;
using VehicleRental.Application.Abstractions.Email;
using VehicleRental.Application.Abstractions.Pagination;
using VehicleRental.Domain.Abstractions;
using VehicleRental.Domain.Rentals;
using VehicleRental.Domain.Users;
using VehicleRental.Domain.Vehicles;
using VehicleRental.Infrastructure.Authentication;
using VehicleRental.Infrastructure.Clock;
using VehicleRental.Infrastructure.Data;
using VehicleRental.Infrastructure.Email;
using VehicleRental.Infrastructure.Email.Settings;
using VehicleRental.Infrastructure.Outbox;
using VehicleRental.Infrastructure.Repositories;

namespace VehicleRental.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IEmailService, EmailService>();
            services.Configure<GmailSettings>(configuration.GetSection("Email:Gmail"));

            var connectionString = configuration.GetConnectionString("Database") ?? throw new ArgumentNullException(nameof(configuration));
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPaginationRepository<User, UserId>, UserRepository>();

            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IPaginationRepository<Vehicle, VehicleId>, VehicleRepository>();

            services.AddScoped<IRentalRepository, RentalRepository>();
            services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());

            services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());


            // Authentication Configuration
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            services.Configure<JwtOptions>(configuration.GetSection("JwtToken"));
            services.ConfigureOptions<JwtBearerOptionsSetup>();
            services.AddTransient<IJwtService, JwtService>();

            services.AddHttpContextAccessor();
            services.AddScoped<IUserContext, UserContext>();

            // Permission Configuration 
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

            // Outbox & BackgroundJobs | Quartz
            services.Configure<OutboxOptions>(configuration.GetSection("Outbox"));
            services.AddQuartz();
            services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
            services.ConfigureOptions<ProcessOutboxMessagesJobSetup>();
            return services;
        }
    }
}
