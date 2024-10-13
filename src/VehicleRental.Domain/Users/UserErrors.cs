using VehicleRental.Domain.Abstractions;

namespace VehicleRental.Domain.Users
{
    public static class UserErrors
    {
        public static Error NotFound = new(
            "User.Found",
            "The user with the specified identifier was not found");

        public static Error InvalidCredentials = new(
            "User.InvalidCredentials",
            "The provided credentials were invalid");

        public static Error AlreadyExists = new(
            "User.AlreadyExists",
            "The user is already registered.");
    }
}
