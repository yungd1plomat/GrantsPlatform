using GrantsPlatform.Abstractions;
using GrantsPlatform.Data;
using GrantsPlatform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GrantsPlatform.Repos
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly JwtOptions _jwtOptions;

        public UserRepository(UserManager<ApplicationUser> userManager, 
            JwtOptions jwtOptions)
        {
            _userManager = userManager;
            _jwtOptions = jwtOptions;
        }

        public async Task<ApplicationUser> AuthenticateAsync(string login, string password)
        {
            var user = await _userManager.FindByNameAsync(login);
            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                return user;
            }
            return null;
        }

        public async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
        {
            IdentityOptions _options = new IdentityOptions();
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                expires: DateTime.Now.AddDays(_jwtOptions.ExpireDays),
                audience: _jwtOptions.Audience,
                claims: claims,
                signingCredentials: new SigningCredentials(_jwtOptions.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
