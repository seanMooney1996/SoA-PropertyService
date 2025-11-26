using Database.DTOs.Landlord;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Database.Models;

namespace Database.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LandlordController : ControllerBase
    {
        private readonly PropertyServiceContext _context;

        public LandlordController(PropertyServiceContext context)
        {
            _context = context;
        }

        // GET: api/Landlord
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LandlordDto>>> GetLandlords()
        {
            var landlords = await _context.Landlords
                .Select(l => new LandlordDto
                {
                    LandlordId = l.Id, 
                    FullName = l.FirstName + " " + l.LastName,
                    Email = l.Email
                })
                .ToListAsync();

            return Ok(landlords);
        }

        // GET: api/Landlord/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LandlordDto>> GetLandlord(Guid id)
        {
            var landlord = await _context.Landlords.FindAsync(id);

            if (landlord == null)
                return NotFound();

            return new LandlordDto
            {
                LandlordId = landlord.Id,
                FullName = landlord.FirstName + " " + landlord.LastName,
                Email = landlord.Email
            };
        }

        // POST: api/Landlord
        [HttpPost]
        public async Task<ActionResult<LandlordDto>> CreateLandlord(CreateLandlordDto dto)
        {
            var landlord = new Landlord
            {
                Id = Guid.NewGuid(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email
            };

            _context.Landlords.Add(landlord);
            await _context.SaveChangesAsync();

            var result = new LandlordDto
            {
                LandlordId = landlord.Id,
                FullName = landlord.FirstName + " " + landlord.LastName,
                Email = landlord.Email
            };

            return CreatedAtAction(nameof(GetLandlord), new { id = landlord.Id }, result);
        }

        // PUT: api/Landlord/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLandlord(Guid id, UpdateLandlordDto dto)
        {
            var landlord = await _context.Landlords.FindAsync(id);

            if (landlord == null)
                return NotFound();

            landlord.FirstName = dto.FirstName ?? landlord.FirstName;
            landlord.LastName = dto.LastName ?? landlord.LastName;
            landlord.Email = dto.Email ?? landlord.Email;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Landlord/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLandlord(Guid id)
        {
            var landlord = await _context.Landlords.FindAsync(id);

            if (landlord == null)
                return NotFound();

            _context.Landlords.Remove(landlord);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
