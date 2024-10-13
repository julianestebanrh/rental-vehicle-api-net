using VehicleRental.Application.Abstractions.Messaging;
using VehicleRental.Domain.Abstractions;
using VehicleRental.Domain.Vehicles;
using VehicleRental.Domain.Vehicles.Specifications;

namespace VehicleRental.Application.Vehicles.GetVechicles
{
    internal sealed class GetVechiclesQueryHandler : IQueryHandler<GetVechiclesQuery, PagedResponse<Vehicle, VehicleId>>
    {
        private readonly IVehicleRepository _vehicleRepository;

        public GetVechiclesQueryHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<Result<PagedResponse<Vehicle, VehicleId>>> Handle(GetVechiclesQuery request, CancellationToken cancellationToken)
        {
            var specification = new VehicleGetAllSpecification(request.Sort!, request.PageIndex, request.PageSize, request.Model!);
            var specificationCount = new VehicleGetAllCountingSpecification(request.Model!);

            var items = await _vehicleRepository.GetAllAsync(specification, cancellationToken);
            var itemsTotal = await _vehicleRepository.CountAsync(specificationCount, cancellationToken);
            var itemsPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(itemsTotal) / Convert.ToDecimal(request.PageSize)));
            var itemsByPage = items.Count;

            return new PagedResponse<Vehicle, VehicleId>
            {
                Count = itemsTotal,
                Items = [.. items],
                PageCount = itemsPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                ItemByPage = itemsByPage
            };

        }
    }
}
