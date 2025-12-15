using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Database.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Database.Models;
using Database.Repositories;
using Database.Services;
using Microsoft.IdentityModel.Tokens;

namespace Database.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationRepository _authRepo;
        private readonly ILandlordRepository _landlordRepo;
        private readonly ITenantRepository _tenantRepo;
        private readonly IJwtService _jwtService;

        public AuthController(
            IAuthenticationRepository authRepo,
            ILandlordRepository landlordRepo,
            ITenantRepository tenantRepo,
            IJwtService jwtService)
        {
            _authRepo = authRepo;
            _landlordRepo = landlordRepo;
            _tenantRepo = tenantRepo;
            _jwtService = jwtService;
        }

        // POST: api/Auth/signup
        [HttpPost("signup")]
        public async Task<ActionResult<AuthResponseDto>> Signup(SignUpDto dto)
        {
            var existing = await _authRepo.GetByEmailAsync(dto.Email);

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

            await _authRepo.AddAsync(auth);

            if (dto.Role.ToLower() == "landlord")
            {
                var landlord = new Landlord
                {
                    Id = auth.Id,
                    Authentication = auth,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    CompanyName = dto.CompanyName,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _landlordRepo.AddAsync(landlord);
            }
            else if (dto.Role.ToLower() == "tenant")
            {
                var tenant = new Tenant
                {
                    Id = auth.Id,
                    Authentication = auth,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName
                };

                await _tenantRepo.AddAsync(tenant);
            }
            else
            {
                return BadRequest("Invalid role. Must be 'tenant' or 'landlord'.");
            }

            await _authRepo.SaveChangesAsync();

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
                FirstName = dto.FirstName,
                Role = auth.Role
            };

            return Ok(response);
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        {
            var user = await _authRepo.GetByEmailAsync(dto.Email);

            if (user == null)
                return Unauthorized("Invalid email or password.");

            if (!PasswordHasher.VerifyPassword(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid email or password.");

            string fName = "";

            if (user.Role.Equals("landlord", StringComparison.OrdinalIgnoreCase))
            {
                var landlord = await _landlordRepo.GetByIdAsync(user.Id);
                fName = landlord?.FirstName ?? "";
            }
            else
            {
                var tenant = await _tenantRepo.GetByIdAsync(user.Id);
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
                FirstName = fName,
                Role = user.Role
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
