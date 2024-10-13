using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleRental.Api.Utils;
using VehicleRental.Application.Users.GetAllUser;
using VehicleRental.Application.Users.GetLoggedInUser;
using VehicleRental.Application.Users.GetUsers;
using VehicleRental.Application.Users.LogInUser;
using VehicleRental.Application.Users.RegisterUser;
using VehicleRental.Domain.Permissions;
using VehicleRental.Infrastructure.Authentication;

namespace VehicleRental.Api.Controllers.Users
{
    [ApiController]
    [ApiVersion(ApiVersions.V1, Deprecated = true)]
    [Route("api/v{version:apiVersion}/users")]
    public class UsersController : ControllerBase
    {
        private readonly ISender _sender;

        public UsersController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("me")]
        [HasPermission(PermissionType.UserRead)]
        public async Task<IActionResult> GetLoggedInUser(CancellationToken cancellationToken)
        {
            var query = new GetLoggedInUserQuery();
            var result = await _sender.Send(query, cancellationToken);
            return Ok(result.Value);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LogInUserRequest request, CancellationToken cancellationToken)
        {
            var command = new LogInUserCommand(request.Email, request.Password);
            var result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return Unauthorized(result.Error);
            }

            return Ok(result.Value);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var command = new RegisterUserCommand(request.Email, request.FirstName, request.LastName, request.Password);
            var result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }


        [AllowAnonymous]
        [HttpGet()]
        public async Task<IActionResult> GetAll([FromQuery] GetUserQuery request)
        {
            var result = await _sender.Send(request);
            return Ok(result.Value);
        }

        [AllowAnonymous]
        [HttpGet("pagination")]
        public async Task<IActionResult> GetAllV2([FromQuery] GetAllUserQuery request)
        {
            var result = await _sender.Send(request);
            return Ok(result.Value);
        }
    }
}
