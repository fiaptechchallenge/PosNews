using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PosNews.Models;
using PosNews.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PosNews.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<bool> RegisterUser(RegisterUser user)
        {
            var identityUser = new IdentityUser
            {
                UserName = user.UserName,
                Email = user.UserName,
                
            };

            var result = await _userManager.CreateAsync(identityUser, user.Password);

            await _userManager.AddToRoleAsync(identityUser, user.Role);

            return result.Succeeded;
        }


        public async Task<bool> Login(LoginUser user)
        {
            var identityUser = await _userManager.FindByEmailAsync(user.UserName);
            
            if(identityUser == null)
            {
                return false;
            }

            return await _userManager.CheckPasswordAsync(identityUser, user.Password);
        }

        public async Task<string> GenerateTokenString(LoginUser user)
        {
            var identityUser =  await _userManager.FindByEmailAsync(user.UserName);
           var roles =  await _userManager.GetRolesAsync(identityUser);


            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Email, user.UserName));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));

            SigningCredentials signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
               claims:claims,
               expires: DateTime.Now.AddMinutes(60),
               issuer:  _configuration.GetSection("Jwt:Issuer").Value,
               audience: _configuration.GetSection("Jwt:Audience").Value,
                signingCredentials: signingCred
               );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return tokenString;
        }
    }
}
