using VehicleRental.Domain.Users;

namespace VehicleRental.Domain.UnitTest.Users
{
    internal class UserMock
    {
        public static readonly FirstName FirstName = new("Alfonso");
        public static readonly LastName LastName = new("Ramos");
        public static readonly Email Email = new("alfonso.ramos@gmail.com");
        public static readonly Password Password = new("Alfonso123$");
    }
}
