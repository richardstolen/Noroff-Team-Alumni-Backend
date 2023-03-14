using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TeamAlumniNETBackend.Data;
using TeamAlumniNETBackend.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        /// Get all Posts related to a user
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns>List of posts</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts([FromHeader] Guid user_id)
        {
            // TODO: FIX Get posts 
            // Returns a list of posts to groups and topics for which the requesting user is subscribed.
            // Optionally accepts appropriate query parameters to search, filter, limit and paginate the
            // number of objects return in the reponse
            return NotFound();
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
        public async Task<ActionResult<Post>> PostPost(Post post)
        {
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

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}
