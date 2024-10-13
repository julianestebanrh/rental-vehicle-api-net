using System.Net.Http.Json;
using VehicleRental.Api.Controllers.Users;
using VehicleRental.Api.FunctionalTest.Users;
using Xunit;

namespace VehicleRental.Api.FunctionalTest.Infrastructure
{
    public class BaseFunctionalTest : IClassFixture<FunctionalTestWebAppFactory>
    {
        protected readonly HttpClient HttpClient;

        protected BaseFunctionalTest(FunctionalTestWebAppFactory factory)
        {
            HttpClient = factory.CreateClient();
        }

        protected async Task<string> GetAccessToken()
        {
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/v1/users/login", new LogInUserRequest(
                UserMock.RegisterTestUserRequest.Email,
                UserMock.RegisterTestUserRequest.Password));

            return await response.Content.ReadAsStringAsync();
        }
    }
}
