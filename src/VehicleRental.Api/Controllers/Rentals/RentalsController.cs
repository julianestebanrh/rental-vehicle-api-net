using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VehicleRental.Api.Utils;
using VehicleRental.Application.Rentals.GetRental;
using VehicleRental.Application.Rentals.ReserveRental;

namespace VehicleRental.Api.Controllers.Rentals
{
    [ApiController]
    [ApiVersion(ApiVersions.V1, Deprecated = true)]
    [Route("api/v{version:apiVersion}/rentals")]
    public class RentalsController : ControllerBase
    {
        private readonly ISender _sender;

        public RentalsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRental(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetRentalQuery(id);

            var result = await _sender.Send(query, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound();
        }

        [HttpPost("reserve")]
        public async Task<IActionResult> ReserveRental(ReserveRentalRequest request, CancellationToken cancellationToken)
        {
            var command = new ReserveRentalCommand(
                request.VehicleId,
                request.UserId,
                request.StartDate,
                request.EndDate);

            var result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return CreatedAtAction(nameof(GetRental), new { id = result.Value }, result.Value);
        }
    }
}
