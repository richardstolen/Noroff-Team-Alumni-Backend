using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TeamAlumniNETBackend.Data;
using TeamAlumniNETBackend.Models;

namespace TeamAlumniNETBackend.Controller
{
    [Authorize]
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
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchEvent(int id, Event @event, [FromHeader] Guid user_id)
        {
            if (user_id != @event.UserId)
            {
                return Forbid();
            }

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

        [HttpDelete("event/event_id/invite/group/group_id")]
        public async Task<IActionResult> DeleteEventGroup(Event _event, [FromHeader] int id, [FromHeader] Guid user_id, [FromHeader] string type)
        {
            var user = await _context.Users.FindAsync(user_id);

            if (user_id != _event.UserId)
            {
                return Forbid();
            }

            if (type != "group")
            {
                return Forbid();
            }

            if (type == "group")
            {
                var group = await _context.Groups.FindAsync(id);

            }

            if (_event == null)
            {
                return NotFound();
            }

            _context.Events.Remove(_event);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("event/event_id/invite/topic/topic_id")]
        public async Task<IActionResult> DeleteEventTopic(Event _event, [FromHeader] int id, [FromHeader] Guid user_id, [FromHeader] string type)
        {
            var user = await _context.Users.FindAsync(user_id);

            if (user_id != _event.UserId)
            {
                return Forbid();
            }

            if (type != "topic")
            {
                return Forbid();
            }

            if (type == "topic")
            {
                var topic = await _context.Topics.FindAsync(id);

            }

            if (_event == null)
            {
                return NotFound();
            }

            _context.Events.Remove(_event);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("event/event_id/invite/user/user_id")]
        public async Task<IActionResult> DeleteEventUser(Event _event, [FromHeader] int id, [FromHeader] Guid user_id, [FromHeader] string type)
        {
            var user = await _context.Users.FindAsync(user_id);

            if (user_id != _event.UserId)
            {
                return Forbid();
            }

            if (type != "user")
            {
                return Forbid();
            }

            if (type == "user")
            {
                var _user = await _context.Users.FindAsync(id);

            }

            if (_event == null)
            {
                return NotFound();
            }

            _context.Events.Remove(_event);
            await _context.SaveChangesAsync();

            return NoContent();
        }




        //[HttpPost("event/event_id/invite/group/group_id")]

        //public async Task <ActionResult<Event>> PostEventGroup(Event _event, [FromHeader] Guid user_id, [FromHeader] string type, [FromHeader] int id) 
        //{
        //    var user = await _context.Users.FindAsync(user_id);

        //    if (user_id != _event.UserId)
        //    {
        //        return Forbid();
        //    }

        //    if (type == "group")
        //    {
        //        var group = await _context.Groups.FindAsync(id);

        //        if (group == null)
        //        {
        //            return NotFound();
        //        }
        //        _event.Groups.Add(group);
        //    }

        //    if (type!= "group") 
        //    {
        //        return Forbid();
        //    }

        //        _context.Events.Add(_event);
        //        await _context.SaveChangesAsync();

        //        return CreatedAtAction("GetEvent", new { id = _event.EventId }, _event);
        //    }

        [HttpPost("event/{event_id}/invite/group/{group_id}")]
        public async Task<ActionResult<Event>> PostEventGroup(int event_id, int group_id, [FromHeader] Guid user_id)
        {
            var _event = await _context.Events.FindAsync(event_id);
            if (_event == null)
            {
                return NotFound();
            }

            if (_event.UserId != user_id)
            {
                return Forbid();
            }

            var group = await _context.Groups.FindAsync(group_id);
            if (group == null)
            {
                return NotFound();
            }

            _event.Groups.Add(group);
            _context.Events.Update(_event);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = _event.EventId }, _event);
        }

        //[HttpPost("event/event_id/invite/topic/topic_id")]

        //public async Task<ActionResult<Event>> PostEventTopic(Event _event, [FromHeader] Guid user_id, [FromHeader] string type, [FromHeader] int id)
        //{
        //    var user = await _context.Users.FindAsync(user_id);

        //    if (user_id != _event.UserId)
        //    {
        //        return Forbid();
        //    }

        //    if (type == "topic")
        //    {
        //        var topic = await _context.Topics.FindAsync(id);

        //        if (topic == null)
        //        {
        //            return NotFound();
        //        }
        //        _event.Topics.Add(topic);
        //    }

        //    if (type != "topic")
        //    {
        //        return Forbid();
        //    }

        //    _context.Events.Add(_event);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetEvent", new { id = _event.EventId }, _event);
        //}

        [HttpPost("event/{event_id}/invite/topic/{topic_id}")]
        public async Task<ActionResult<Event>> PostEventTopic(int event_id, int topic_id, [FromHeader] Guid user_id)
        {
            var _event = await _context.Events.FindAsync(event_id);
            if (_event == null)
            {
                return NotFound();
            }

            if (_event.UserId != user_id)
            {
                return Forbid();
            }

            var topic = await _context.Topics.FindAsync(topic_id);
            if (topic == null)
            {
                return NotFound();
            }

            _event.Topics.Add(topic);
            _context.Events.Update(_event);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = _event.EventId }, _event);
        }


        [HttpPost("event/event_id/invite/user/user_id")]

        public async Task<ActionResult<Event>> PostEventUser(Event _event, [FromHeader] Guid user_id, [FromHeader] string type)
        {
            var user = await _context.Users.FindAsync(user_id);

            if (user_id != _event.UserId)
            {
                return Forbid();
            }

            if (type == "user")
            {
                if (user == null)
                {
                    return NotFound();
                }
                user.Events.Add(_event);
            }

            if (type != "user")
            {
                return Forbid();
            }


            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = _event.EventId }, _event);
        }


        [HttpPost("event/event_id/rsvp")]

        public async Task<ActionResult<Event>> PostEventRsvp(Event _event, [FromHeader] Guid user_id, [FromHeader] string type, int id)
        {
            var user = await _context.Users.FindAsync(user_id);

            if (user_id != _event.UserId)
            {
                return Forbid();
            }

            if (type == "rsvp")
            {
                var rsvp = await _context.Rsvps.FindAsync(id);

                if (user == null)
                {
                    return NotFound();
                }
                _event.Rsvps.Add(rsvp);
            }

            if (type != "rsvp")
            {
                return Forbid();
            }

            _context.Events.Add(_event);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = _event.EventId }, _event);
        }



        [HttpPost("event")]
        public async Task<ActionResult<Event>> PostEvent(Event _event, [FromHeader] Guid user_id, [FromHeader] string type, [FromHeader] int id)
        {

            var user = await _context.Users.FindAsync(user_id);

            if (type == "topic")
            {
                var topic = await _context.Topics.FindAsync(id);

                if (topic == null)
                {
                    return NotFound();
                }
                _event.Topics.Add(topic);
            }

            if (type == "group")
            {
                var group = await _context.Groups.FindAsync(id);

                if (group == null)
                {
                    return NotFound();
                }
                _event.Groups.Add(group);
            }

            if (type == "rsvp")
            {
                var rsvp = await _context.Rsvps.FindAsync(id);

                if (rsvp == null)
                {
                    return NotFound();
                }
                _event.Rsvps.Add(rsvp);
            }

            if (type == "user")
            {
                var _user = await _context.Users.FindAsync(id);

                if (_user == null)
                {
                    return NotFound();
                }
                _event.Users.Add(_user);
            }

            //DateTime now = DateTime.Now;
            //_event.LastUpdate = now;
            _context.Events.Add(_event);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = _event.EventId }, _event);
        }
    }
}




