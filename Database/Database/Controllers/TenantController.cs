using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Database.DTOs.Property;
using Database.DTOs.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Database.Models;
using Microsoft.AspNetCore.Authorization;

namespace Database.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly PropertyServiceContext _context;

        public TenantController(PropertyServiceContext context)
        {
            _context = context;
        }

        // GET: api/Tenant
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tenant>>> GetTenants()
        {
            return await _context.Tenants.ToListAsync();
        }

        // GET: api/Tenant/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tenant>> GetTenant(Guid id)
        {
            var tenant = await _context.Tenants.FindAsync(id);

            if (tenant == null)
            {
                return NotFound();
            }

            return tenant;
        }

        // PUT: api/Tenant/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTenant(Guid id, Tenant tenant)
        {
            if (id != tenant.Id)
            {
                return BadRequest();
            }

            _context.Entry(tenant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TenantExists(id))
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

        // POST: api/Tenant
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tenant>> PostTenant(Tenant tenant)
        {
            _context.Tenants.Add(tenant);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTenant", new { id = tenant.Id }, tenant);
        }

        // DELETE: api/Tenant/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTenant(Guid id)
        {
            var tenant = await _context.Tenants.FindAsync(id);
            if (tenant == null)
            {
                return NotFound();
            }

            _context.Tenants.Remove(tenant);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TenantExists(Guid id)
        {
            return _context.Tenants.Any(e => e.Id == id);
        }
        
        // GET /Tenant/myRental
        [Authorize]
        [HttpGet("myRental")]
        public async Task<ActionResult<PropertyDto>> GetMyRental()
        {
            var tenantId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var property = await _context.Properties
                .Where(p => p.TenantId == tenantId)
                .FirstOrDefaultAsync();

            if (property == null)
                return Ok(null); 

            return new PropertyDto
            {
                Id = property.Id,
                AddressLine1 = property.AddressLine1!,
                City = property.City!,
                County = property.County!,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                RentPrice = property.RentPrice,
            };
        }
        
        // GET /Tenant/openRentals
        [Authorize]
        [HttpGet("openRentals")]
        public async Task<ActionResult<IEnumerable<PropertyDto>>> GetOpenRentals()
        {
            var properties = await _context.Properties
                .Where(p => p.TenantId == null)
                .Select(p => new PropertyDto
                {
                    Id = p.Id,
                    AddressLine1 = p.AddressLine1!,
                    City = p.City!,
                    County = p.County!,
                    Bedrooms = p.Bedrooms,
                    Bathrooms = p.Bathrooms,
                    RentPrice = p.RentPrice
                })
                .ToListAsync();
        
            return Ok(properties);
        }
        
        
        [Authorize]
        [HttpPost("request/{propertyId:guid}")]
        public async Task<ActionResult<RentalRequest>> CreateRentalRequest(Guid propertyId)
        {
            var tenantIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (tenantIdString == null)
                return Unauthorized("No tenant id in token");
    
            var tenantId = Guid.Parse(tenantIdString);
            
            // Check to see if tenant is already renting a property. Tenants can only rent once
            var isAlreadyRenting = await _context.Properties
                .AnyAsync(p => p.TenantId == tenantId);

            if (isAlreadyRenting)
                return BadRequest("You are currently renting a property and cannot request a new one.");
            
            
            //find property. Must have no tenant for request to be made
            var property = await _context.Properties
                .FirstOrDefaultAsync(p => p.Id == propertyId && p.TenantId == null);
            
            if (property == null)
                return BadRequest("Property not available.");
            
            // If tenant already requested for property
            var existingRequest = await _context.RentalRequests
                .AnyAsync(r => r.TenantId == tenantId && r.PropertyId == propertyId && r.Status == "Pending");

            if (existingRequest)
                return BadRequest("You already have a pending request for this property.");
            
            var request = new RentalRequest
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                PropertyId = propertyId,
                Status = "Pending",
                RequestedAt = DateTime.UtcNow
            };
            
            _context.RentalRequests.Add(request);
            await _context.SaveChangesAsync();
    
            return Ok(new RentalRequestDto
            {
                Id = request.Id,
                PropertyId = request.PropertyId,
                TenantId = request.TenantId,
                Status = request.Status,
                RequestedAt = request.RequestedAt,
                City = property.City,    
                County = property.County,
                Address = property.AddressLine1, 
                TenantName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value,
            });
        }
        
        [Authorize]
        [HttpGet("request/myRequests")]
        public async Task<ActionResult<RentalRequest>> GetMyRequests()
        {
            var tenantIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (tenantIdString == null)
                return Unauthorized("No tenant id in token");
        
            var tenantId = Guid.Parse(tenantIdString);
            
            var requests = await _context.RentalRequests
                .Where(r => r.TenantId == tenantId)
                .OrderByDescending(r => r.RequestedAt)
                .Select(r => new RentalRequestDto
                {
                    Id = r.Id,
                    PropertyId = r.PropertyId,
                    Address = r.Property.AddressLine1!,
                    City = r.Property.City!,
                    County = r.Property.County!,
                    TenantId = r.TenantId,
                    TenantName = (r.Tenant.FirstName + " " + r.Tenant.LastName).Trim(),
                    Status = r.Status,
                    RequestedAt = r.RequestedAt
                })
                .ToListAsync();
            
            return Ok(requests);
        }
        
        [Authorize]
        [HttpPost("request/{propertyId:guid}/cancel")]
        public async Task<ActionResult<RentalRequest>> CancelRentalRequest(Guid propertyId)
        {
            var tenantIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (tenantIdStr == null)
                return Unauthorized("No tenant ID in token");
        
            var tenantId = Guid.Parse(tenantIdStr);
            
            var request = await _context.RentalRequests
                .FirstOrDefaultAsync(r => r.TenantId == tenantId && r.PropertyId == propertyId);

            if (request == null)
                return NotFound("No request found for this property.");

            _context.RentalRequests.Remove(request);
            await _context.SaveChangesAsync();
            
            return Ok(new { message = "Request cancelled successfully." });
        }
    }
}
