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

            var user = new Authentication
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                PasswordHash = PasswordHasher.HashPassword(dto.Password),
                Role = "User",
                CreatedAt = DateTime.UtcNow
            };

            _context.Authentications.Add(user);
            await _context.SaveChangesAsync();

            var response = new AuthResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                Token = _jwtService.GenerateToken(user.Id, user.Email) 
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

            var response = new AuthResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                Token = _jwtService.GenerateToken(user.Id,user.Email)
            };

            return Ok(response);
        }
    }
}
