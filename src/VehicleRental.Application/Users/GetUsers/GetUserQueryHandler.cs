using Dapper;
using System.Text;
using VehicleRental.Application.Abstractions.Data;
using VehicleRental.Application.Abstractions.Messaging;
using VehicleRental.Domain.Abstractions;

namespace VehicleRental.Application.Users.GetUsers
{
    internal sealed class GetUserQueryHandler : IQueryHandler<GetUserQuery, PagedResultDapper<GetUserDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetUserQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<PagedResultDapper<GetUserDto>>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            var builder = new StringBuilder("""
                SELECT
                   usr.email, rl.name as role, p.name as permission
                FROM users usr
                    LEFT JOIN users_roles usr_rl 
                        ON usr.id = usr_rl.user_id
                    LEFT JOIN roles rl 
                        ON rl.id = usr_rl.role_id
                    LEFT JOIN roles_permissions rp 
                        ON rl.id = rp.role_id
                    LEFT JOIN permissions p 
                        ON p.id = rp.permission_id
                """);

            var search = string.Empty;
            var whereStatement = string.Empty;

            if (!string.IsNullOrEmpty(request.Search))
            {
                search = "%" + EncodeForLike(request.Search) + "%";
                whereStatement = $" WHERE rl.name LIKE @Search ";
                builder.AppendLine(whereStatement);
            }

            var orderBy = request.OrderBy;
            if (!string.IsNullOrEmpty(orderBy))
            {
                var orderStatement = string.Empty;
                var orderAsc = request.OrderAsc ? "ASC" : "DESC";

                orderStatement = orderBy switch
                {
                    "email" => $" ORDER BY usr.email {orderAsc}",
                    "role" => $" ORDER BY usr.name {orderAsc}",
                    _ => $" ORDER BY usr.name {orderAsc}",
                };

                builder.AppendLine(orderStatement);
            }

            builder.AppendLine(" LIMIT @PageSize OFFSET @Offset;");

            builder.AppendLine("""
                SELECT
                   COUNT(*)
                FROM users usr
                    LEFT JOIN users_roles usr_rl 
                        ON usr.id = usr_rl.user_id
                    LEFT JOIN roles rl 
                        ON rl.id = usr_rl.role_id
                    LEFT JOIN roles_permissions rp 
                        ON rl.id = rp.role_id
                    LEFT JOIN permissions p 
                        ON p.id = rp.permission_id
                """);

            builder.AppendLine(whereStatement);
            builder.AppendLine(";");

            var offset = request.PageSize * (request.PageIndex - 1);
            var sql = builder.ToString();
            using var multi = await connection.QueryMultipleAsync(sql, new { PageSize = request.PageSize, Offset = offset, Search = search });

            var items = await multi.ReadAsync<GetUserDto>().ConfigureAwait(false);
            var totalItems = await multi.ReadFirstAsync<int>().ConfigureAwait(false);

            var result = new PagedResultDapper<GetUserDto>(totalItems, request.PageIndex, request.PageSize)
            {
                Items = items
            };

            return result;
        }

        private string EncodeForLike(string search)
        {
            return search.Replace("[", "[]]").Replace("%", "[%]");
        }
    }
}
