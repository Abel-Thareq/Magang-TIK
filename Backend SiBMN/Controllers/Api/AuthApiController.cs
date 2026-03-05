using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SiBMN.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SiBMN.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public AuthApiController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public class LoginRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Unit)
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.Password == request.Password);

            if (user == null)
                return Unauthorized(new { message = "Email atau password salah" });

            var key = _config["Jwt:Key"] ?? "SiBMN_SuperSecretKey_2026_VeryLongKeyForSecurity!";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("UserId", user.IdUser.ToString()),
                new Claim("UserName", user.Nama),
                new Claim("UserEmail", user.Email),
                new Claim("RoleId", user.RoleId.ToString()),
                new Claim("RoleName", user.Role?.NamaRole ?? ""),
                new Claim("UnitId", user.UnitId.ToString()),
                new Claim("UnitName", user.Unit?.NamaUnit ?? "")
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: credentials
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                user = new
                {
                    userId = user.IdUser,
                    nama = user.Nama,
                    email = user.Email,
                    roleId = user.RoleId,
                    roleName = user.Role?.NamaRole ?? "",
                    unitId = user.UnitId,
                    unitName = user.Unit?.NamaUnit ?? ""
                }
            });
        }

        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return Unauthorized();

            try
            {
                var token = authHeader.Substring("Bearer ".Length);
                var key = _config["Jwt:Key"] ?? "SiBMN_SuperSecretKey_2026_VeryLongKeyForSecurity!";
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                if (jsonToken == null) return Unauthorized();

                return Ok(new
                {
                    userId = int.Parse(jsonToken.Claims.First(c => c.Type == "UserId").Value),
                    nama = jsonToken.Claims.First(c => c.Type == "UserName").Value,
                    email = jsonToken.Claims.First(c => c.Type == "UserEmail").Value,
                    roleId = int.Parse(jsonToken.Claims.First(c => c.Type == "RoleId").Value),
                    roleName = jsonToken.Claims.First(c => c.Type == "RoleName").Value,
                    unitId = int.Parse(jsonToken.Claims.First(c => c.Type == "UnitId").Value),
                    unitName = jsonToken.Claims.First(c => c.Type == "UnitName").Value
                });
            }
            catch
            {
                return Unauthorized();
            }
        }
    }
}
