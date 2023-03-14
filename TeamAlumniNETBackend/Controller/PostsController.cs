using System;
using System.Collections.Generic;
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
    public class PostsController : ControllerBase
    {
        private readonly AlumniDbContext _context;

        public PostsController(AlumniDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Methode to get all posts to a spesific user 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of posts</returns>
        [HttpGet("get/post/user")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts([FromHeader] Guid id)
        {
            var postList = await _context.Posts.Where(post => post.TargetUser == id).ToListAsync();

            if (postList == null)
            {
                return NotFound();
            }

            return postList;
        }

        /// <summary>
        /// methode to get all posts between two users 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="targetUser"></param>
        /// <returns>List of posts</returns>

        [HttpGet("get/post/user/user_id")]

        public async Task<ActionResult<IEnumerable<Post>>> GetPostsByID(Guid userId , [FromHeader] Guid targetUser)
        {
            var directPost = await _context.Posts.Where(Post => Post.TargetUser == targetUser).Where(Post => Post.UserId == userId).ToListAsync();
          

            if (directPost == null) 
            {
                return NotFound();
            }
            return directPost;
        }

        /// <summary>
        /// methode to get all posts in a spesific group
        /// </summary>
        /// <param name="targetGroup"></param>
        /// <returns>List of posts</returns>

        [HttpGet("post/group/group_id")]
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
        /// Methode to get all posts in a spesific Topic
        /// </summary>
        /// <param name="targetTopic"></param>
        /// <returns>List of posts</returns>

        [HttpGet("Post/Topic/topic_id")]
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
        /// Methode to get all posts to a spesific event
        /// </summary>
        /// <param name="targetEvent"></param>
        /// <returns>List of posts</returns>

        [HttpGet("post/event/event_id")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsByEvent([FromHeader] int targetEvent) 
        {
            var eventPost = await _context.Posts.Where(Post => Post.TargetEvent== targetEvent).ToListAsync();

            if (eventPost == null) 
            {
                return NotFound();
            }
            return eventPost;
        }






        // GET: api/Posts/5
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



        // PUT: api/Posts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        // POST: api/Posts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPost", new { id = post.PostId }, post);
        }

        // DELETE: api/Posts/5
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
