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
using Database.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace Database.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly ITenantRepository _tenantRepo;
        private readonly IPropertyRepository _propertyRepo;
        private readonly IRentalRequestRepository _requestRepo;

        public TenantController(
            ITenantRepository tenantRepo,
            IPropertyRepository propertyRepo,
            IRentalRequestRepository requestRepo)
        {
            _tenantRepo = tenantRepo;
            _propertyRepo = propertyRepo;
            _requestRepo = requestRepo;
        }

        // GET: api/Tenant
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tenant>>> GetTenants()
        {
            var tenants = await _tenantRepo.GetAllAsync();
            return Ok(tenants);
        }

        // GET: api/Tenant/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tenant>> GetTenant(Guid id)
        {
            var tenant = await _tenantRepo.GetByIdAsync(id);

            if (tenant == null)
            {
                return NotFound();
            }

            return tenant;
        }

        // POST: api/Tenant
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tenant>> PostTenant(Tenant tenant)
        {
            await _tenantRepo.AddAsync(tenant);
            await _tenantRepo.SaveChangesAsync();

            return CreatedAtAction("GetTenant", new { id = tenant.Id }, tenant);
        }

        // GET /Tenant/myRental
        [Authorize]
        [HttpGet("myRental")]
        public async Task<ActionResult<PropertyDto>> GetMyRental()
        {
            var tenantId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var property = await _propertyRepo.GetPropertyByTenantIdAsync(tenantId);

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
            var properties = await _propertyRepo.GetOpenPropertiesAsync();

            var dtos = properties.Select(p => new PropertyDto
            {
                Id = p.Id,
                AddressLine1 = p.AddressLine1!,
                City = p.City!,
                County = p.County!,
                Bedrooms = p.Bedrooms,
                Bathrooms = p.Bathrooms,
                RentPrice = p.RentPrice
            });
        
            return Ok(dtos);
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
            var isAlreadyRenting = await _propertyRepo.IsTenantRentingAnyPropertyAsync(tenantId);

            if (isAlreadyRenting)
                return BadRequest("You are currently renting a property and cannot request a new one.");
            
            //find property. Must have no tenant for request to be made
            var property = await _propertyRepo.GetByIdAsync(propertyId);
            
            if (property == null || property.TenantId != null)
                return BadRequest("Property not available.");
            
            // If tenant already requested for property
            var existingRequest = await _requestRepo.HasPendingRequestAsync(tenantId, propertyId);

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
            
            await _requestRepo.AddAsync(request);
            await _requestRepo.SaveChangesAsync();
    
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
                TenantName = User.FindFirst(ClaimTypes.Name)?.Value,
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
            
            var requests = await _requestRepo.GetByTenantIdWithDetailsAsync(tenantId);

            var dtos = requests.Select(r => new RentalRequestDto
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
            });
            
            return Ok(dtos);
        }
        
        [Authorize]
        [HttpPost("request/{propertyId:guid}/cancel")]
        public async Task<ActionResult<RentalRequest>> CancelRentalRequest(Guid propertyId)
        {
            var tenantIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (tenantIdStr == null)
                return Unauthorized("No tenant ID in token");
        
            var tenantId = Guid.Parse(tenantIdStr);
            
            var request = await _requestRepo.GetByTenantAndPropertyIdAsync(tenantId, propertyId);

            if (request == null)
                return NotFound("No request found for this property.");

            await _requestRepo.DeleteAsync(request);
            await _requestRepo.SaveChangesAsync();
            
            return Ok(new { message = "Request cancelled successfully." });
        }
    }
}
