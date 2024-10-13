using VehicleRental.Api.Controllers.Users;

namespace VehicleRental.Api.FunctionalTest.Users
{
    internal static class UserMock
    {
        public static RegisterUserRequest RegisterTestUserRequest = new("test@test.com", "test", "test", "Test12345$");
    }
}
