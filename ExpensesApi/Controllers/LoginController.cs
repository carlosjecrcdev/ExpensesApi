using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using ExpensesApi.Models.Dtos;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using ExpensesApi.Interfaces;

namespace ExpensesApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserAccountServices _userAccountServices;

        public LoginController(IConfiguration configuration, IUserAccountServices userAccountServices)
        {
            _configuration = configuration;
            _userAccountServices = userAccountServices;
        }

        [HttpPost("Authenticate")]
        public async Task<IActionResult> Login(UserAccountDto user)
        {
            var userAccount = await _userAccountServices.GetById(user.Id);

            if (userAccount == null || userAccount.IsAdmin == false)
                        return Unauthorized();


            string token = Generatetoken(user);

            return Ok(new { token});
                
        }

        private string Generatetoken(UserAccountDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Audience = audience,
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
