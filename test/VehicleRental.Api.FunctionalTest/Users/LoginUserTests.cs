﻿using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using VehicleRental.Api.Controllers.Users;
using VehicleRental.Api.FunctionalTest.Infrastructure;
using Xunit;

namespace VehicleRental.Api.FunctionalTest.Users
{
    public class LoginUserTests : BaseFunctionalTest
    {
        private const string Email = "login@test.com";
        private const string Password = "12345";

        public LoginUserTests(FunctionalTestWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new LogInUserRequest(Email, Password);

            // Act
            var response = await HttpClient.PostAsJsonAsync("api/v1/users/login", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Login_ShouldReturnOk_WhenUserExists()
        {
            // Arrange
            var registerRequest = new RegisterUserRequest(Email, "first", "last", Password);
            await HttpClient.PostAsJsonAsync("api/v1/users/register", registerRequest);

            var request = new LogInUserRequest(Email, Password);

            // Act
            var response = await HttpClient.PostAsJsonAsync("api/v1/users/login", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}