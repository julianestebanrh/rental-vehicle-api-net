using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using VehicleRental.Api.Utils;
using VehicleRental.Application.Vehicles.GetAllVehicles;
using VehicleRental.Application.Vehicles.GetVechicles;
using VehicleRental.Application.Vehicles.ReportVehicle;
using VehicleRental.Application.Vehicles.SearchVehicles;
using VehicleRental.Domain.Permissions;
using VehicleRental.Infrastructure.Authentication;

namespace VehicleRental.Api.Controllers.Vehicles
{
    [ApiController]
    [ApiVersion(ApiVersions.V1, Deprecated = true)]
    [Route("api/v{version:apiVersion}/vehicles")]
    public class VehiclesController : ControllerBase
    {
        private readonly ISender _sender;

        public VehiclesController(ISender sender)
        {
            _sender = sender;
        }

        [HasPermissionAttribute(PermissionType.UserRead)]
        [HttpGet("search")]
        public async Task<IActionResult> SearchVehicles(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken)
        {
            var query = new SearchVehiclesQuery(startDate, endDate);
            var result = await _sender.Send(query, cancellationToken);
            return Ok(result.Value);
        }

        [AllowAnonymous]
        [HttpGet()]
        public async Task<IActionResult> GetAll([FromQuery] GetVechiclesQuery request)
        {
            var result = await _sender.Send(request);
            return Ok(result.Value);
        }

        [AllowAnonymous]
        [HttpGet("pagination", Name = "Pagination")]
        public async Task<IActionResult> GetAllV2([FromQuery] GetAllVehiclesQuery request)
        {
            var result = await _sender.Send(request);
            return Ok(result.Value);
        }

        [AllowAnonymous]
        [HttpGet("report")]
        public async Task<IActionResult> ReportVehicles(CancellationToken cancellationToken, string model = "")
        {
            var query = new ReportVehicleQuery(model);
            var result = await _sender.Send(query, cancellationToken);
            byte[] pdfBytes = result.Value.GeneratePdf();
            return File(pdfBytes, "application/pdf", "report_vehicles.pdf");
        }

    }
}
