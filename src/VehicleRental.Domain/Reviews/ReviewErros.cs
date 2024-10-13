using VehicleRental.Domain.Abstractions;

namespace VehicleRental.Domain.Reviews
{
    public static class ReviewErrors
    {
        public static readonly Error NotEligible = new(
            "Review.NotEligible",
            "The review is not eligible because the rental is not yet completed");
    }
}
