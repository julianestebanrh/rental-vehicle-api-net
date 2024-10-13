using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleRental.Domain.Shared;
using VehicleRental.Domain.Vehicles;

namespace VehicleRental.Infrastructure.Configurations
{
    internal sealed class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.ToTable("vehicles");

            builder.HasKey(vehicle => vehicle.Id);

            builder.Property(vehicle => vehicle.Id)
                .HasConversion(vehicleId => vehicleId!.Value, value => new VehicleId(value));

            builder.OwnsOne(vehicle => vehicle.Address);

            builder.Property(vehicle => vehicle.Model)
                .HasConversion(model => model!.Value, value => new Model(value))
                .HasMaxLength(200);

            builder.Property(vehicle => vehicle.Vin)
               .HasConversion(vin => vin!.Value, value => new Vin(value))
               .HasMaxLength(500);

            builder.OwnsOne(vehicle => vehicle.Price, priceBuilder =>
            {
                priceBuilder.Property(money => money.Currency)
                    .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
            });

            builder.OwnsOne(apartment => apartment.MaintenanceFee, priceBuilder =>
            {
                priceBuilder.Property(money => money.Currency)
                    .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
            });

            builder.Property<uint>("Version").IsRowVersion();
        }
    }
}
