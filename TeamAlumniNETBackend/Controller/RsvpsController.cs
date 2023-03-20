using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamAlumniNETBackend.Data;
using TeamAlumniNETBackend.Models;

namespace TeamAlumniNETBackend.Controller
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RsvpsController : ControllerBase
    {
        private readonly AlumniDbContext _context;

        public RsvpsController(AlumniDbContext context)
        {
            _context = context;
        }

        // GET: api/Rsvps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rsvp>>> GetRsvps()
        {
            return await _context.Rsvps.ToListAsync();
        }

        // GET: api/Rsvps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rsvp>> GetRsvp(int id)
        {
            var rsvp = await _context.Rsvps.FindAsync(id);

            if (rsvp == null)
            {
                return NotFound();
            }

            return rsvp;
        }

        // PUT: api/Rsvps/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRsvp(int id, Rsvp rsvp)
        {
            if (id != rsvp.RsvpId)
            {
                return BadRequest();
            }

            _context.Entry(rsvp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RsvpExists(id))
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

        // POST: api/Rsvps
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Rsvp>> PostRsvp(Rsvp rsvp)
        {
            _context.Rsvps.Add(rsvp);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRsvp", new { id = rsvp.RsvpId }, rsvp);
        }

        // DELETE: api/Rsvps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRsvp(int id)
        {
            var rsvp = await _context.Rsvps.FindAsync(id);
            if (rsvp == null)
            {
                return NotFound();
            }

            _context.Rsvps.Remove(rsvp);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RsvpExists(int id)
        {
            return _context.Rsvps.Any(e => e.RsvpId == id);
        }
    }
}
