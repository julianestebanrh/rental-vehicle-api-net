using VehicleRental.Domain.Abstractions;

namespace VehicleRental.Domain.Rentals
{
    public static class RentalErrors
    {
        public static Error NotFound = new(
            "Rental.Found",
            "The rental with the specified identifier was not found");

        public static Error Overlap = new(
            "Rental.Overlap",
            "The current rental is overlapping with an existing one");

        public static Error NotReserved = new(
            "Rental.NotReserved",
            "The rental is not reserved");

        public static Error NotConfirmed = new(
            "Rental.NotReserved",
            "The rental is not confirmed");

        public static Error AlreadyStarted = new(
            "Rental.AlreadyStarted",
            "The rental has already started");
    }
}
