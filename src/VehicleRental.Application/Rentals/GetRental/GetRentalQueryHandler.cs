using Dapper;
using VehicleRental.Application.Abstractions.Data;
using VehicleRental.Application.Abstractions.Messaging;
using VehicleRental.Domain.Abstractions;

namespace VehicleRental.Application.Rentals.GetRental
{
    internal sealed class GetRentalQueryHandler : IQueryHandler<GetRentalQuery, RentalResponse>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetRentalQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<RentalResponse>> Handle(GetRentalQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
            SELECT
                id AS Id,
                vehicle_id AS VehicleId,
                user_id AS UserId,
                status AS Status,
                price_for_period_amount AS PriceAmount,
                price_for_period_currency AS PriceCurrency,
                maintenance_fee_amount AS MaintenanceFeeAmount,
                maintenance_fee_currency AS MaintenanceFeeCurrency,
                amenities_up_charge_amount AS AmenitiesUpChargeAmount,
                amenities_up_charge_currency AS AmenitiesUpChargeCurrency,
                total_price_amount AS TotalPriceAmount,
                total_price_currency AS TotalPriceCurrency,
                duration_start AS DurationStart,
                duration_end AS DurationEnd,
                created_on_utc AS CreatedOnUtc
            FROM rentals
            WHERE id = @RentalId
            """;

            var rental = await connection.QueryFirstOrDefaultAsync<RentalResponse>(sql, new { request.RentalId });

            return rental;
        }
    }
}
