using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Database.DTOs.Property;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Database.Models;
using Microsoft.AspNetCore.Authorization;

namespace Database.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly PropertyServiceContext _context;

        public PropertyController(PropertyServiceContext context)
        {
            _context = context;
        }

        // GET: api/Property
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Property>>> GetProperties()
        {
            return await _context.Properties.ToListAsync();
        }

        // GET: api/Property/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Property>> GetProperty(Guid id)
        {
            var @property = await _context.Properties.FindAsync(id);

            if (@property == null)
            {
                return NotFound();
            }

            return @property;
        }

        // PUT: api/Property/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProperty(Guid id, Property @property)
        {
            if (id != @property.Id)
            {
                return BadRequest();
            }

            _context.Entry(@property).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PropertyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Property
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Property>> PostProperty(CreatePropertyDto propertyDto)
        {
            var property = new Property()
            {
                Id =  Guid.NewGuid(),
                LandlordId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                TenantId = null,
                AddressLine1 = propertyDto.AddressLine1,
                City = propertyDto.City,
                County = propertyDto.County,
                Bedrooms = propertyDto.Bedrooms,
                Bathrooms = propertyDto.Bathrooms,
                RentPrice = propertyDto.RentPrice,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Properties.Add(property);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProperty", new { id = property.Id }, property);
        }

        // DELETE: api/Property/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProperty(Guid id)
        {
            var @property = await _context.Properties.FindAsync(id);
            if (@property == null)
            {
                return NotFound();
            }

            if (!@property.IsAvailable)
            {
                return Conflict("Property cannot be deleted while occupied or unavailable.");
            }

            _context.Properties.Remove(@property);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PropertyExists(Guid id)
        {
            return _context.Properties.Any(e => e.Id == id);
        }
    }
}
