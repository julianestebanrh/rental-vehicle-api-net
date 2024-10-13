using FluentAssertions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using VehicleRental.Api.FunctionalTest.Infrastructure;
using VehicleRental.Application.Users.GetLoggedInUser;
using Xunit;

namespace VehicleRental.Api.FunctionalTest.Users
{
    public class GetLoggedInUserTests : BaseFunctionalTest
    {
        public GetLoggedInUserTests(FunctionalTestWebAppFactory factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Get_ShouldReturnUnauthorized_WhenAccessTokenIsMissing()
        {
            // Act
            var response = await HttpClient.GetAsync("api/v1/users/me");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Get_ShouldReturnUser_WhenAccessTokenIsNotMissing()
        {
            // Arrange
            var accessToken = await GetAccessToken();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, accessToken);

            // Act
            var user = await HttpClient.GetFromJsonAsync<UserResponse>("api/v1/users/me");

            // Assert
            user.Should().NotBeNull();
        }
    }

}
