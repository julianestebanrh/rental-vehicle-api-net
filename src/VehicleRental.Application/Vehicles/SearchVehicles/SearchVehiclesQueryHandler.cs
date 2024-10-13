using Dapper;
using VehicleRental.Application.Abstractions.Data;
using VehicleRental.Application.Abstractions.Messaging;
using VehicleRental.Domain.Abstractions;
using VehicleRental.Domain.Rentals;

namespace VehicleRental.Application.Vehicles.SearchVehicles
{
    internal sealed class SearchVehiclesQueryHandler : IQueryHandler<SearchVehiclesQuery, IReadOnlyList<VehicleResponse>>
    {
        private static readonly int[] ActiveRentalStatuses =
            {
                (int)RentalStatus.Reserved,
                (int)RentalStatus.Confirmed,
                (int)RentalStatus.Completed
            };

        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public SearchVehiclesQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<IReadOnlyList<VehicleResponse>>> Handle(SearchVehiclesQuery request, CancellationToken cancellationToken)
        {
            if (request.StartDate > request.EndDate)
            {
                return new List<VehicleResponse>();
            }

            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
            SELECT
                a.id AS Id,
                a.model AS Model,
                a.vin AS Vin,
                a.price_amount AS Price,
                a.price_currency AS Currency,
                a.address_country AS Country,
                a.address_department AS Department,
                a.address_zip_code AS ZipCode,
                a.address_city AS City,
                a.address_street AS Street
            FROM vehicles AS a
            WHERE NOT EXISTS
            (
                SELECT 1
                FROM rentals AS b
                WHERE
                    b.vehicle_id = a.id AND
                    b.duration_start <= @EndDate AND
                    b.duration_end >= @StartDate AND
                    b.status = ANY(@ActiveRentalStatuses)
            )
            """;

            var vehicles = await connection
                .QueryAsync<VehicleResponse, AddressResponse, VehicleResponse>(
                    sql,
                    (vehicle, address) =>
                    {
                        vehicle.Address = address;

                        return vehicle;
                    },
                    new
                    {
                        request.StartDate,
                        request.EndDate,
                        ActiveRentalStatuses
                    },
            splitOn: "Country");

            return vehicles.ToList();
        }
    }
}
