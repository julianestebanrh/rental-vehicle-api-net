using Dapper;
using VehicleRental.Application.Abstractions.Authentication;
using VehicleRental.Application.Abstractions.Data;
using VehicleRental.Application.Abstractions.Messaging;
using VehicleRental.Domain.Abstractions;

namespace VehicleRental.Application.Users.GetLoggedInUser
{
    internal sealed class GetLoggedInUserQueryHandler : IQueryHandler<GetLoggedInUserQuery, UserResponse>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly IUserContext _userContext;

        public GetLoggedInUserQueryHandler(ISqlConnectionFactory sqlConnectionFactory, IUserContext userContext)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _userContext = userContext;
        }

        public async Task<Result<UserResponse>> Handle(
            GetLoggedInUserQuery request,
            CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
            SELECT
                id AS Id,
                first_name AS FirstName,
                last_name AS LastName,
                email AS Email
            FROM users
            WHERE id = @Id
            """;

            var user = await connection.QuerySingleAsync<UserResponse>(
                sql,
                new
                {
                    Id = _userContext.UserId
                });

            return user;
        }
    }
}
