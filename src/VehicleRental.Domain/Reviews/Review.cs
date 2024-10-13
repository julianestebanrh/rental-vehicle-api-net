using VehicleRental.Domain.Abstractions;
using VehicleRental.Domain.Rentals;
using VehicleRental.Domain.Reviews.Events;
using VehicleRental.Domain.Users;
using VehicleRental.Domain.Vehicles;

namespace VehicleRental.Domain.Reviews
{
    public class Review : Entity<ReviewId>
    {
        private Review() { }
        private Review(
            ReviewId id,
            VehicleId vehicleId,
            RentalId rentalId,
            UserId userId,
            Rating rating,
            Comment comment,
            DateTime createdOnUtc)
          : base(id)
        {
            VehicleId = vehicleId;
            RentalId = rentalId;
            UserId = userId;
            Rating = rating;
            Comment = comment;
            CreatedOnUtc = createdOnUtc;
        }

        public VehicleId? VehicleId { get; private set; }

        public RentalId? RentalId { get; private set; }

        public UserId? UserId { get; private set; }

        public Rating? Rating { get; private set; }

        public Comment? Comment { get; private set; }

        public DateTime CreatedOnUtc { get; private set; }

        public static Result<Review> Create(
            Rental rental,
            Rating rating,
            Comment comment,
            DateTime createdOnUtc)
        {
            if (rental.Status != RentalStatus.Completed)
            {
                return Result.Failure<Review>(ReviewErrors.NotEligible);
            }

            var review = new Review(
                ReviewId.New(),
                rental.VehicleId!,
                rental.Id!,
                rental.UserId!,
                rating,
                comment,
                createdOnUtc);

            review.RaiseDomainEvent(new ReviewCreatedDomainEvent(review.Id!));

            return review;
        }
    }
}
