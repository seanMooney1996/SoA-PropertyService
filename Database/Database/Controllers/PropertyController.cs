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
using Database.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace Database.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyRepository _propertyRepo;

        public PropertyController(IPropertyRepository propertyRepo)
        {
            _propertyRepo = propertyRepo;
        }

        // GET: api/Property
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Property>>> GetProperties()
        {
            var properties = await _propertyRepo.GetAllAsync();
            return Ok(properties);
        }

        // GET: api/Property/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Property>> GetProperty(Guid id)
        {
            var property = await _propertyRepo.GetByIdAsync(id);

            if (property == null)
            {
                return NotFound();
            }

            return property;
        }

        // POST: api/Property
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Property>> PostProperty(CreatePropertyDto propertyDto)
        {
            var property = new Property()
            {
                Id = Guid.NewGuid(),
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

            await _propertyRepo.AddAsync(property);
            await _propertyRepo.SaveChangesAsync();

            return CreatedAtAction("GetProperty", new { id = property.Id }, property);
        }

        // DELETE: api/Property/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProperty(Guid id)
        {
            var property = await _propertyRepo.GetByIdAsync(id);
            if (property == null)
            {
                return NotFound();
            }
            if (!property.IsAvailable)
            {
                return Conflict("Property cannot be deleted while occupied");
            }

            await _propertyRepo.DeleteAsync(property);
            await _propertyRepo.SaveChangesAsync();

            return NoContent();
        }
    }
}
