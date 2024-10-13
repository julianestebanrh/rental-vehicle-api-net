using FluentValidation;

namespace VehicleRental.Application.Rentals.ReserveRental
{
    internal class ReserveRentalCommandValidator : AbstractValidator<ReserveRentalCommand>
    {
        public ReserveRentalCommandValidator()
        {
            RuleFor(c => c.UserId).NotEmpty();

            RuleFor(c => c.VehicleId).NotEmpty();

            RuleFor(c => c.StartDate).LessThan(c => c.EndDate);
        }
    }
}
