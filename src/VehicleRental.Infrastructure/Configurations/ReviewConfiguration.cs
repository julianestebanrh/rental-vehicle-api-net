using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleRental.Domain.Rentals;
using VehicleRental.Domain.Reviews;
using VehicleRental.Domain.Users;
using VehicleRental.Domain.Vehicles;

namespace VehicleRental.Infrastructure.Configurations
{
    internal sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("reviews");

            builder.Property(review => review.Id)
                .HasConversion(reviewId => reviewId!.Value, value => new ReviewId(value));

            builder.HasKey(review => review.Id);

            builder.Property(review => review.Rating)
                .HasConversion(rating => rating!.Value, value => Rating.Create(value).Value);

            builder.Property(review => review.Comment)
                .HasMaxLength(200)
                .HasConversion(comment => comment!.Value, value => new Comment(value));

            builder.HasOne<Vehicle>()
                .WithMany()
                .HasForeignKey(review => review.VehicleId);

            builder.HasOne<Rental>()
                .WithMany()
                .HasForeignKey(review => review.RentalId);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(review => review.UserId);
        }
    }
}
