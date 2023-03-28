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
    [Route("/topic")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly AlumniDbContext _context;

        public TopicsController(AlumniDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all topics.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Topic>>> GetTopics()
        {
            return await _context.Topics.ToListAsync();
        }

        /// <summary>
        /// Get Topic by ID.
        /// </summary>
        /// <param name="topic_id"></param>
        /// <returns></returns>
        [HttpGet("{topic_id}")]
        public async Task<ActionResult<Topic>> GetTopic(int topic_id)
        {
            var topic = await _context.Topics.FindAsync(topic_id);

            if (topic == null)
            {
                return NotFound();
            }

            return topic;
        }

        /// <summary>
        /// Edit topic.
        /// </summary>
        /// <param name="topic_id"></param>
        /// <param name="topic"></param>
        /// <returns></returns>
        [HttpPut("{topic_id}")]
        public async Task<IActionResult> PutTopic(int topic_id, Topic topic)
        {
            if (topic_id != topic.TopicId)
            {
                return BadRequest();
            }

            _context.Entry(topic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TopicExists(topic_id))
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
        /// Create new topic.
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName(nameof(GetTopic))]
        public async Task<ActionResult<Topic>> PostTopic(Topic topic)
        {
            _context.Topics.Add(topic);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTopic), new { id = topic.TopicId }, topic);
        }

        /// <summary>
        /// Delete topic by ID.
        /// </summary>
        /// <param name="topic_id"></param>
        /// <returns></returns>
        [HttpDelete("{topic_id}")]
        public async Task<IActionResult> DeleteTopic(int topic_id)
        {
            var topic = await _context.Topics.FindAsync(topic_id);
            if (topic == null)
            {
                return NotFound();
            }

            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Add a user to topic.
        /// </summary>
        /// <param name="topic_id"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        [HttpPost("{topic_id}/join")]
        public async Task<IActionResult> AddUserToTopic(int topic_id, [FromHeader] Guid user_id)
        {
            // Get User and Topic
            var user = await _context.Users.FindAsync(user_id);
            var topic = await _context.Topics.FindAsync(topic_id);

            // Handle Not Found
            if (topic == null || user == null)
            {
                return NotFound();
            }

            // Add User to Topic
            topic.Users.Add(user);

            // Save
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Remove user from Topic.
        /// </summary>
        /// <param name="topic_id"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        [HttpPost("/topic/{topic_id}/remove")]
        public async Task<IActionResult> RemoveUserFromTopic(int topic_id, [FromHeader] Guid user_id)
        {
            // Load the parent entity that contains the collection of child entities
            var parentEntity = _context.Topics.Include(p => p.Users).FirstOrDefault(p => p.TopicId == topic_id);

            if (parentEntity == null)
            {
                return NotFound();
            }

            // Get the child entity that you want to delete
            var childEntity = parentEntity.Users.FirstOrDefault(c => c.UserId == user_id);

            // Remove the child entity from the parent entity's collection of child entities
            parentEntity.Users.Remove(childEntity);

            // Save the changes to the database
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Check if topic exists.
        /// </summary>
        /// <param name="topic_id"></param>
        /// <returns></returns>
        private bool TopicExists(int topic_id)
        {
            return _context.Topics.Any(e => e.TopicId == topic_id);
        }
    }
}
