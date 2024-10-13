using MediatR;
using VehicleRental.Application.Rentals.GetRental;
using VehicleRental.Application.Rentals.ReserveRental;

namespace VehicleRental.Api.Controllers.Rentals
{
    public static class RentalsEndpoints
    {
        public static void MapRentalEndpoints(this IEndpointRouteBuilder builder)
        {

            builder.MapGet("rentals/{id}", GetRental)
                .WithName(nameof(GetRental))
                .MapToApiVersion(2);

            builder.MapPost("rentals/reserve", ReserveRental)
                .WithName(nameof(ReserveRental))
                .MapToApiVersion(2);

        }

        public static async Task<IResult> GetRental(Guid id, ISender sender, CancellationToken cancellationToken)
        {
            var query = new GetRentalQuery(id);
            var result = await sender.Send(query, cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
        }

        public static async Task<IResult> ReserveRental(ReserveRentalRequest request, ISender sender, CancellationToken cancellationToken)
        {
            var command = new ReserveRentalCommand(
                request.VehicleId,
                request.UserId,
                request.StartDate,
                request.EndDate);

            var result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }

            return Results.CreatedAtRoute(nameof(GetRental), new { id = result.Value }, result.Value);
        }
    }
}
