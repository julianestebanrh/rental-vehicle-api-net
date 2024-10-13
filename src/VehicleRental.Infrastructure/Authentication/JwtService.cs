using Dapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VehicleRental.Application.Abstractions.Authentication;
using VehicleRental.Application.Abstractions.Data;
using VehicleRental.Domain.Users;

namespace VehicleRental.Infrastructure.Authentication
{
    internal sealed class JwtService : IJwtService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public JwtService(IOptions<JwtOptions> jwtOptions, ISqlConnectionFactory sqlConnectionFactory)
        {
            _jwtOptions = jwtOptions.Value;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<string> GetAccessTokenAsync(User user)
        {
            const string sql = """
                SELECT
                    p.name
                FROM users usr 
                    LEFT JOIN users_roles usr_rl ON usr.id = usr_rl.user_id
                    LEFT JOIN roles rl ON rl.id = usr_rl.role_id
                    LEFT JOIN roles_permissions rp ON rl.id = rp.role_id
                    LEFT JOIN permissions p ON p.id = rp.permission_id
                    WHERE usr.id = @UserId
                """;

            using var connection = _sqlConnectionFactory.CreateConnection();
            var permissions = await connection.QueryAsync<string>(sql, new { UserId = user.Id!.Value });
            var permissionCollection = permissions.ToHashSet();

            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, $"{user.Id.Value}"),
                new (JwtRegisteredClaimNames.Email, user.Email!.Value),
            };

            foreach (var permission in permissionCollection)
            {
                claims.Add(new(Claims.Permissions, permission));
            }

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey!)), SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_jwtOptions.Issuer!, _jwtOptions.Audience!, claims, null, DateTime.UtcNow.AddHours(1), signingCredentials);
            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }
    }
}
