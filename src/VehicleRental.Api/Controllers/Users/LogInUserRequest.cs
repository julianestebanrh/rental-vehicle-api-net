﻿namespace VehicleRental.Api.Controllers.Users
{
    public sealed record LogInUserRequest(
        string Email,
        string Password);
}
