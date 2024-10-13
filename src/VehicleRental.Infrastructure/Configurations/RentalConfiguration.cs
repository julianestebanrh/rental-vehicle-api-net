using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleRental.Domain.Rentals;
using VehicleRental.Domain.Shared;
using VehicleRental.Domain.Users;
using VehicleRental.Domain.Vehicles;

namespace VehicleRental.Infrastructure.Configurations
{
    internal sealed class RentalConfiguration : IEntityTypeConfiguration<Rental>
    {
        public void Configure(EntityTypeBuilder<Rental> builder)
        {
            builder.ToTable("rentals");

            builder.HasKey(rental => rental.Id);

            builder.Property(rental => rental.Id)
                .HasConversion(rentalId => rentalId!.Value, value => new RentalId(value));

            builder.OwnsOne(rental => rental.PriceForPeriod, priceBuilder =>
            {
                priceBuilder.Property(money => money.Currency)
                    .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
            });

            builder.OwnsOne(rental => rental.MaintenanceFee, priceBuilder =>
            {
                priceBuilder.Property(money => money.Currency)
                    .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
            });

            builder.OwnsOne(rental => rental.AmenitiesUpCharge, priceBuilder =>
            {
                priceBuilder.Property(money => money.Currency)
                    .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
            });

            builder.OwnsOne(rental => rental.TotalPrice, priceBuilder =>
            {
                priceBuilder.Property(money => money.Currency)
                    .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
            });

            builder.OwnsOne(rental => rental.Duration);

            builder.HasOne<Vehicle>()
                .WithMany()
                .HasForeignKey(rental => rental.VehicleId);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(rental => rental.UserId);
        }
    }
}
