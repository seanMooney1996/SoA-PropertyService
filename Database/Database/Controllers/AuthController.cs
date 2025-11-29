using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Database.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Database.Models;
using Database.Services;
using Microsoft.IdentityModel.Tokens;

namespace Database.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly PropertyServiceContext _context;
        private readonly JwtService _jwtService;

        public AuthController(PropertyServiceContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // POST: api/Auth/signup
        [HttpPost("signup")]
        public async Task<ActionResult<AuthResponseDto>> Signup(SignUpDto dto)
        {
            var existing = await _context.Authentications
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (existing != null)
                return Conflict("Email already in use.");

            var auth = new Authentication
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                PasswordHash = PasswordHasher.HashPassword(dto.Password),
                Role = dto.Role.ToLower(),
                CreatedAt = DateTime.UtcNow
            };

            _context.Authentications.Add(auth);

            if (dto.Role.ToLower() == "landlord")
            {
                var landlord = new Landlord
                {
                    Id = auth.Id,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    CompanyName = dto.CompanyName,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Landlords.Add(landlord);
            }
            else if (dto.Role.ToLower() == "tenant")
            {
                var tenant = new Tenant
                {
                    Id = auth.Id,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName
                };

                _context.Tenants.Add(tenant);
            }
            else
            {
                return BadRequest("Invalid role. Must be 'tenant' or 'landlord'.");
            }

            await _context.SaveChangesAsync();

            var token = _jwtService.GenerateToken(auth.Id, auth.Email, auth.Role);

            Response.Cookies.Append(
                "authToken",
                token,
                new CookieOptions
                {
                    Path = "/",               
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddHours(1)
                }
            );

            var response = new AuthResponseDto
            {
                UserId = auth.Id,
                Email = auth.Email,
                FirstName = dto.FirstName
            };

            return Ok(response);
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        {
            var user = await _context.Authentications
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null)
                return Unauthorized("Invalid email or password.");

            if (!PasswordHasher.VerifyPassword(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid email or password.");

            string fName = "";

            if (user.Role.Equals("landlord", StringComparison.OrdinalIgnoreCase))
            {
                var landlord = await _context.Landlords
                    .FirstOrDefaultAsync(x => x.Id == user.Id);
                fName = landlord?.FirstName ?? "";
            }
            else
            {
                var tenant = await _context.Tenants
                    .FirstOrDefaultAsync(x => x.Id == user.Id);
                fName = tenant?.FirstName ?? "";
            }

            var token = _jwtService.GenerateToken(user.Id, user.Email, user.Role);

            Response.Cookies.Append(
                "authToken",
                token,
                new CookieOptions
                {
                    Path = "/",
                    HttpOnly = true,
                    Secure = true,              
                    SameSite = SameSiteMode.None, 
                    Expires = DateTime.UtcNow.AddHours(1)
                }
            );
            
            var response = new AuthResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = fName
            };

            return Ok(response);
        }

        // POST: api/Auth/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Append(
                "authToken",
                "",
                new CookieOptions
                {
                    Path = "/",
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(-1)
                }
            );

            return Ok(new { message = "Logged out" });
        }
    }
}
