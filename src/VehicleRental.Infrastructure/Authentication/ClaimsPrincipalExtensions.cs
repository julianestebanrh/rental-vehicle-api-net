using System.Security.Claims;

namespace VehicleRental.Infrastructure.Authentication
{
    internal static class ClaimsPrincipalExtensions
    {
        public static string GetUserEmail(this ClaimsPrincipal? principal)
        {
            return principal?.FindFirstValue(ClaimTypes.Email) ??
                throw new ApplicationException("User identity is unavailable");

        }

        public static Guid GetUserId(this ClaimsPrincipal? principal)
        {
            var userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.TryParse(userId, out var parsedUserId) ?
                parsedUserId :
                throw new ApplicationException("User id is unavailable");
        }

        //public static string GetIdentityId(this ClaimsPrincipal? principal)
        //{
        //    return principal?.FindFirstValue(ClaimTypes.NameIdentifier) ??
        //           throw new ApplicationException("User identity is unavailable");
        //}
    }

}
