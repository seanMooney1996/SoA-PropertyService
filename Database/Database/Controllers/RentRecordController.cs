using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Database.Models;
using Microsoft.AspNetCore.Authorization;

namespace Database.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentRecordController : ControllerBase
    {
        private readonly PropertyServiceContext _context;

        public RentRecordController(PropertyServiceContext context)
        {
            _context = context;
            context.Database.EnsureCreated();
        }

        // GET: api/RentRecord
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RentRecord>>> GetRentRecords()
        {
            return await _context.RentRecords.ToListAsync();
        }

        // GET: api/RentRecord/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<RentRecord>> GetRentRecord(Guid id)
        {
            var rentRecord = await _context.RentRecords.FindAsync(id);

            if (rentRecord == null)
            {
                return NotFound();
            }

            return rentRecord;
        }

        // PUT: api/RentRecord/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRentRecord(Guid id, RentRecord rentRecord)
        {
            if (id != rentRecord.Id)
            {
                return BadRequest();
            }

            _context.Entry(rentRecord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentRecordExists(id))
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

        // POST: api/RentRecord
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<RentRecord>> PostRentRecord(RentRecord rentRecord)
        {
            _context.RentRecords.Add(rentRecord);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRentRecord", new { id = rentRecord.Id }, rentRecord);
        }

        // DELETE: api/RentRecord/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRentRecord(Guid id)
        {
            var rentRecord = await _context.RentRecords.FindAsync(id);
            if (rentRecord == null)
            {
                return NotFound();
            }

            _context.RentRecords.Remove(rentRecord);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RentRecordExists(Guid id)
        {
            return _context.RentRecords.Any(e => e.Id == id);
        }
    }
}
