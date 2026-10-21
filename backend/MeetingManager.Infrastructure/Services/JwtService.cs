using MeetingManager.Core.DTO;
using MeetingManager.Core.ServiceContracts;
using MeetingManager.Domain.Entities;
using MeetingManager.Domain.RepositoryContracts;
using MeetingManager.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MeetingManager.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly JwtOptions _jwtOptions;
        private readonly RefreshTokenOptions _refreshTokenOptions;


        public JwtService(IRoleRepository roleRepository, IOptions<JwtOptions> jwtOptions, IOptions<RefreshTokenOptions> refreshtokenOptions)
        {
            _roleRepository = roleRepository;
            _jwtOptions = jwtOptions.Value;
            _refreshTokenOptions = refreshtokenOptions.Value;
        }

        public async Task<AuthResponse> GenerateJwtToken(User user, CancellationToken ct = default)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!)
            };

            // Adding roles
            var roles = await _roleRepository.GetUserRolesAsync(user.Id, ct);
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r.Name)));

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtOptions.Secret)
                );

            SigningCredentials signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256
                );

            var jwtExpirationDate = DateTimeOffset.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes);

            JwtSecurityToken token = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                expires: jwtExpirationDate.UtcDateTime,
                signingCredentials: signingCredentials
                );

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var tokenString = jwtSecurityTokenHandler.WriteToken(token);

            return new AuthResponse()
            {
                JwtToken = tokenString,
                JwtTokenExpiration = jwtExpirationDate,
                RefreshToken = GenerateRefreshToken(),
                RefreshTokenExpiration = DateTimeOffset.UtcNow.AddMinutes(_refreshTokenOptions.ExpirationMinutes)
            };
        }

        private static string GenerateRefreshToken()
                => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}
