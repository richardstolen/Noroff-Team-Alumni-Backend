using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamAlumniNETBackend.Data;
using TeamAlumniNETBackend.Models;

namespace TeamAlumniNETBackend.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly AlumniDbContext _context;

        public EventsController(AlumniDbContext context)
        {
            _context = context;
        }

        [HttpGet("/event")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents([FromHeader] Guid user_id)
        {
            var eventList = await _context.Events.Where(Event => Event.UserId == user_id).ToListAsync();

            if (eventList == null)
            {
                return NotFound();
            }
            return (eventList);
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }

        // PUT: api/Events/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, Event @event)
        {
            if (id != @event.EventId)
            {
                return BadRequest();
            }

            _context.Entry(@event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
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

        // POST: api/Events
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(Event @event)
        {
            _context.Events.Add(@event);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = @event.EventId }, @event);
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }

        [HttpPost("event")]
        public async Task<ActionResult<Event>> PostEvent(Event _event, [FromHeader] Guid user_id, [FromHeader] string type)
        {
            Debug.WriteLine($"user: {user_id}");

            var user = await _context.Users.FindAsync(user_id);
            var topicType = await _context.Topics.FindAsync(type);
            var exists = false;
            

            if (topicType != null) 
            {
                var topic = _context.Topics.Where(topic => topic.Events == _event.Topics);
            }



            if (_event.Topics != null)
            {
                // Check if user is in topic
                var topic = _context.Topics.Where(topic => topic.Events == _event.Topics);

                if (topic == null)
                {
                    return NotFound();
                }
                exists = topic.Any(topic => topic.Users.Contains(user));

            }
            if (_event.Groups != null)
            {
                // Check if user is in group
                var group = _context.Groups.Where(group => group.Events == _event.Groups);
                if (group == null)
                {
                    return NotFound();
                }
                exists = group.Any(group => group.Users.Contains(user));
            }

            if (!exists)
            {
                return Forbid();
            }

            //DateTime now = DateTime.Now;
            //_event.LastUpdate = now;
            //_context.Events.Add(_event);
            //await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = _event.EventId }, _event);
        }

    }
}
