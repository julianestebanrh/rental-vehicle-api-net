using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using VehicleRental.Application.Abstractions.Behaviors;
using VehicleRental.Domain.Rentals;

namespace VehicleRental.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            services.AddAutoMapper(typeof(DependencyInjection).Assembly);

            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

                configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));

                configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));

                //configuration.AddOpenBehavior(typeof(QueryCachingBehavior<,>));
            });

            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);

            services.AddTransient<PricingService>();

            return services;

        }
    }
}
