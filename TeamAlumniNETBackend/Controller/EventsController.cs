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
    [Route("/event")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly AlumniDbContext _context;

        public EventsController(AlumniDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all evntes by user_id
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns>Events</returns>

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

        /// <summary>
        /// Get event by Event id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Event</returns>
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

        /// <summary>
        /// Update an event
        /// </summary>
        /// <param name="id"></param>
        /// <param name="event"></param>
        /// <param name="user_id"></param>
        /// <returns>Updated event</returns>
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

       

        /// <summary>
        /// Delete event by Event id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Delete an evnt by Group id
        /// </summary>
        /// <param name="_event"></param>
        /// <param name="id"></param>
        /// <param name="user_id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpDelete("event_id/invite/group/group_id")]
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

        /// <summary>
        /// Delte an event by Topic id
        /// </summary>
        /// <param name="_event"></param>
        /// <param name="id"></param>
        /// <param name="user_id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpDelete("event_id/invite/topic/topic_id")]
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

        /// <summary>
        /// Delete an event by User id
        /// </summary>
        /// <param name="_event"></param>
        /// <param name="id"></param>
        /// <param name="user_id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpDelete("event_id/invite/user/user_id")]
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


        /// <summary>
        /// Create an event for a spesific group
        /// </summary>
        /// <param name="event_id"></param>
        /// <param name="group_id"></param>
        /// <param name="user_id"></param>
        /// <returns>Event to a spesific group</returns>
        [HttpPost("event_id/invite/group/group_id")]
        public async Task<ActionResult<Event>> PostEventGroup([FromHeader] int event_id,  [FromHeader] int group_id, [FromHeader] Guid user_id)
        {
            var _event = await _context.Events.FindAsync(event_id);

            foreach (var exist in _event.Groups)
            {
                if (exist.GroupId == group_id)
                {
                    return Conflict();
                }
            }

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

        /// <summary>
        /// Create an event for a spesific topic
        /// </summary>
        /// <param name="event_id"></param>
        /// <param name="topic_id"></param>
        /// <param name="user_id"></param>
        /// <returns>Event to a spesific topic</returns>
        [HttpPost("event_id/invite/topic/topic_id")]
        public async Task<ActionResult<Event>> PostEventTopic([FromHeader] int event_id, [FromHeader] int topic_id, [FromHeader] Guid user_id)
        {
            var _event = await _context.Events.FindAsync(event_id);

            foreach (var exist in _event.Topics)
            {
                if (exist.TopicId == topic_id)
                {
                    return Conflict();
                }
            }

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

        /// <summary>
        /// Create an event for a spesific user
        /// </summary>
        /// <param name="event_id"></param>
        /// <param name="user_id"></param>
        /// <param name="type"></param>
        /// <returns>Event to a spesific user</returns>
        [HttpPost("event_id/invite/user/user_id")]

        public async Task<ActionResult<Event>> PostEventUser([FromHeader] int event_id, [FromHeader] Guid user_id, [FromHeader] string type)
        {
            var user = await _context.Users.FindAsync(user_id);
            var _event = await _context.Events.FindAsync(event_id);

            if (user_id != _event.UserId)
            {
                return Forbid();
            }

            foreach (var exist in _event.Users)
            {
                if (exist.UserId == user_id)
                {
                    return Conflict();
                }
            }

            if (type == "user")
            {
                if (user == null)
                {
                    return NotFound();
                }

                EventUser eventUser = new EventUser();

                eventUser.Event = _event;
                eventUser.EventId = _event.EventId;
                eventUser.UserId = user.UserId;
                eventUser.User = user;

                user.Events.Add(eventUser);
            }

            if (type != "user")
            {
                return Forbid();
            }


            await _context.SaveChangesAsync();

            return NoContent();
        }


        /// <summary>
        /// Create an event for a spesific user, group or topic
        /// </summary>
        /// <param name="_event"></param>
        /// <param name="user_id"></param>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns>Event to a spesific user, group or topic</returns>
        [HttpPost("")]
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


            if (type == "user")
            {
                var _user = await _context.Users.FindAsync(id);

                if (_user == null)
                {
                    return NotFound();
                }

                EventUser eventUser = new EventUser();

                eventUser.Event = _event;
                eventUser.EventId = _event.EventId;
                eventUser.UserId = _user.UserId;
                eventUser.User = _user;

                _event.Users.Add(eventUser);
            }

            _context.Events.Add(_event);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = _event.EventId }, _event);
        }
    }
}




