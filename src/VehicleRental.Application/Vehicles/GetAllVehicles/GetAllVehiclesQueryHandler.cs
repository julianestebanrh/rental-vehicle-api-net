using AutoMapper;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using VehicleRental.Application.Abstractions.Messaging;
using VehicleRental.Application.Abstractions.Pagination;
using VehicleRental.Domain.Abstractions;
using VehicleRental.Domain.Vehicles;

namespace VehicleRental.Application.Vehicles.GetAllVehicles
{
    internal sealed class GetAllVehiclesQueryHandler : IQueryHandler<GetAllVehiclesQuery, PagedResult<GetAllVehicleQueryDto>>
    {
        private readonly IMapper _mapper;
        private readonly IPaginationRepository<Vehicle, VehicleId> _paginationRepository;

        public GetAllVehiclesQueryHandler(IPaginationRepository<Vehicle, VehicleId> paginationRepository, IMapper mapper)
        {
            _paginationRepository = paginationRepository;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<GetAllVehicleQueryDto>>> Handle(GetAllVehiclesQuery request, CancellationToken cancellationToken)
        {
            var builder = PredicateBuilder.New<Vehicle>(true);

            if (!string.IsNullOrEmpty(request.Search))
            {
                builder = builder.Or(p => p.Model == new Model(request.Search));
                builder = builder.Or(p => p.Vin == new Vin(request.Search));
            }

            var vehicleQueryable = _paginationRepository.GetAllAsync(
                builder,
                p => p.Include(x => x.Address!),
                request.PageIndex,
                request.PageSize,
                request.OrderBy!,
                request.OrderAsc
            );

            var result = await PagedResult<GetAllVehicleQueryDto>.CreateAsync(vehicleQueryable, request.PageIndex, request.PageSize, _mapper);

            return result;
        }
    }
}
