using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MSClubInsights.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;

        public TokenService(IConfiguration config , UserManager<AppUser> userManager)
        {
            _config = config;
            _userManager = userManager;
        }

        public string GenerateToken(AppUser user)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
           };
        }
    }
}
