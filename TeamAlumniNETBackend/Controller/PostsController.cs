using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper.Execution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TeamAlumniNETBackend.Data;
using TeamAlumniNETBackend.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Group = TeamAlumniNETBackend.Models.Group;

namespace TeamAlumniNETBackend.Controller
{
    [Route("/post")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly AlumniDbContext _context;

        public PostsController(AlumniDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all Posts related to a user. This means all posts from groups, topics and events 
        /// that the user is subscribed to.
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns>List of posts</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts([FromHeader] Guid user_id)
        {
            // Get the User
            var user = await _context.Users.FindAsync(user_id);

            // Get Groups, topics and events that the user is subscribed to
            var groups = await _context.Groups.Where(g => g.Users.Contains(user)).ToListAsync();
            var topics = await _context.Topics.Where(t => t.Users.Contains(user)).ToListAsync();
            var events = await _context.Events.Where(e => e.Users.Contains(user)).ToListAsync();

            var postList = new List<Post>();

            /*
             *  Looping through all groups, topics and events and getting all posts from them.
             */
            foreach (var group in groups)
            {
                var groupPosts = await _context.Posts.Where(p => p.TargetGroup == group.GroupId).ToListAsync();
                postList.AddRange(groupPosts);
            }

            foreach (var topic in topics)
            {
                var topicPosts = await _context.Posts.Where(p => p.TargetTopic == topic.TopicId).ToListAsync();
                postList.AddRange(topicPosts);
            }

            foreach (var @event in events)
            {
                var eventPosts = await _context.Posts.Where(e => e.TargetEvent == @event.EventId).ToListAsync();
                postList.AddRange(eventPosts);
            }

            if (postList == null)
            {
                return NotFound();
            }

            // Sorting the list, with last updated post first
            postList.Sort((x, y) => DateTime.Compare(y.LastUpdate, x.LastUpdate));

            return postList;
        }

        /// <summary>
        /// Get all Direct messages to a spesific user 
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns>List of posts</returns>
        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<Post>>> GetDirectMessages([FromHeader] Guid user_id)
        {
            var postList = await _context.Posts.Where(post => post.TargetUser == user_id).ToListAsync();

            if (postList == null)
            {
                return NotFound();
            }
            return postList;
        }

        /// <summary>
        /// Get all Direct Messages between two users 
        /// </summary>
        /// <param name="userId">The user that the current user wants messages from</param>
        /// <param name="targetUser">Current or logged in user</param>
        /// <returns>List of posts</returns>
        [HttpGet("user/{user_id}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsByID(Guid userId, [FromHeader] Guid targetUser)
        {
            var directPost = await _context.Posts.Where(Post => Post.TargetUser == targetUser).Where(Post => Post.UserId == userId).ToListAsync();

            if (directPost == null)
            {
                return NotFound();
            }
            return directPost;
        }

        /// <summary>
        /// Get all posts in a spesific group
        /// </summary>
        /// <param name="targetGroup"></param>
        /// <returns>List of posts</returns>
        [HttpGet("group/group_id")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsByGroup([FromHeader] int targetGroup)
        {
            var groupPost = await _context.Posts.Where(Post => Post.TargetGroup == targetGroup).ToListAsync();

            if (groupPost == null)
            {
                return NotFound();
            }
            return groupPost;
        }

        /// <summary>
        /// Get all posts in a spesific Topic
        /// </summary>
        /// <param name="targetTopic"></param>
        /// <returns>List of posts</returns>
        [HttpGet("topic/topic_id")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsByTopic([FromHeader] int targetTopic)
        {
            var topicPost = await _context.Posts.Where(Post => Post.TargetTopic == targetTopic).ToListAsync();

            if (topicPost == null)
            {
                return NotFound();
            }
            return topicPost;
        }

        /// <summary>
        /// Get all posts to a spesific event
        /// </summary>
        /// <param name="targetEvent"></param>
        /// <returns>List of posts</returns>
        [HttpGet("event/event_id")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsByEvent([FromHeader] int targetEvent)
        {
            var eventPost = await _context.Posts.Where(Post => Post.TargetEvent == targetEvent).ToListAsync();

            if (eventPost == null)
            {
                return NotFound();
            }
            return eventPost;
        }

        /// <summary>
        /// Get Post by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        /// <summary>
        /// Edit a post.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, Post post)
        {
            if (id != post.PostId)
            {
                return BadRequest();
            }

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
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
        /// Create a new Post.
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(Post post, [FromHeader] Guid user_id)
        {
            var user = await _context.Users.FindAsync(user_id);
            var exists = false;

            if (post.TargetEvent != null)
            {
                // Check if user is in event
                var _event = _context.Events.Where(_event => _event.EventId == post.TargetEvent);

                if (_event == null)
                {
                    return NotFound();
                }
                exists = _event.Any(_event => _event.Users.Contains(user));
            }
            if (post.TargetTopic != null)
            {
                // Check if user is in topic
                var topic = _context.Topics.Where(topic => topic.TopicId == post.TargetTopic);

                if (topic == null)
                {
                    return NotFound();
                }
                exists = topic.Any(topic => topic.Users.Contains(user));

            }
            if (post.TargetGroup != null)
            {
                // Check if user is in group
                var group = _context.Groups.Where(group => group.GroupId == post.TargetGroup);
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

            DateTime now = DateTime.Now;
            post.LastUpdate = now;
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPost", new { id = post.PostId }, post);
        }

        /// <summary>
        /// Delete a post.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Check if post exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}
