using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamAlumniNETBackend.Data;
using TeamAlumniNETBackend.DTOs.PostDTOs;
using TeamAlumniNETBackend.Models;

namespace TeamAlumniNETBackend.Controller
{
    [Route("/post")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly AlumniDbContext _context;
        private readonly IMapper _mapper;
        public PostsController(IMapper mapper, AlumniDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all Posts related to a user. This means all posts from groups, topics and events 
        /// that the user is subscribed to.
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns>List of posts</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostGetDTO>>> GetPosts([FromHeader] Guid user_id)
        {
            // Get the User
            var user = await _context.Users.FindAsync(user_id);

            EventUser eventUser = new EventUser();
            eventUser.UserId = user.UserId;
            eventUser.User = user;

            // Get Groups, topics and events that the user is subscribed to
            var groups = await _context.Groups.Where(g => g.Users.Contains(user)).ToListAsync();
            var topics = await _context.Topics.Where(t => t.Users.Contains(user)).ToListAsync();
            var events = await _context.Events.Where(e => e.Users.Contains(eventUser)).ToListAsync();

            var targetNames = new List<string>();
            var postListTuple = new List<Tuple<Post, string>>();

            /*
             *  Looping through all groups, topics and events and getting all posts from them.
             */
            foreach (var group in groups)
            {
                var groupPosts = await _context.Posts.Where(p => p.TargetGroup == group.GroupId).ToListAsync();

                foreach (var post in groupPosts)
                {
                    var postTuple = new Tuple<Post, string>(post, group.Name == null ? "NO NAME" : group.Name);
                    postListTuple.Add(postTuple);
                }
            }

            foreach (var topic in topics)
            {
                var topicPosts = await _context.Posts.Where(p => p.TargetTopic == topic.TopicId).ToListAsync();
                foreach (var post in topicPosts)
                {
                    var postTuple = new Tuple<Post, string>(post, topic.Name == null ? "NO NAME" : topic.Name);
                    postListTuple.Add(postTuple);
                }
            }

            foreach (var @event in events)
            {
                var eventPosts = await _context.Posts.Where(e => e.TargetEvent == @event.EventId).ToListAsync();
                foreach (var post in eventPosts)
                {
                    var postTuple = new Tuple<Post, string>(post, @event.Description == null ? "NO NAME" : @event.Description);
                    postListTuple.Add(postTuple);
                }
            }

            var postDTOList = new List<PostGetDTO>();

            for (int i = 0; i < postListTuple.Count; i++)
            {
                var postDTO = new PostGetDTO();
                postDTO.PostId = postListTuple[i].Item1.PostId;
                var createdBy = await _context.Users.FindAsync(postListTuple[i].Item1.UserId);
                postDTO.CreatedBy = createdBy?.UserName;
                postDTO.LastUpdate = postListTuple[i].Item1.LastUpdate;
                postDTO.Body = postListTuple[i].Item1.Body;
                postDTO.Title = postListTuple[i].Item1.Title;
                postDTO.Target = postListTuple[i].Item2;
                var comments = await _context.Posts.Where(p => p.TargetPost == postListTuple[i].Item1.PostId).ToListAsync();

                foreach (var comment in comments)
                {
                    var commentDTO = new CommentDTO();
                    commentDTO.PostId = comment.PostId;
                    var _createdBy = await _context.Users.FindAsync(comment.UserId);
                    commentDTO.CreatedBy = _createdBy.UserName;
                    commentDTO.Target = comment.TargetPost;
                    commentDTO.LastUpdate = comment.LastUpdate;
                    commentDTO.Body = comment.Body;
                    commentDTO.Title = comment.Title;
                    postDTO.Comments.Add(commentDTO);
                }

                postDTOList.Add(postDTO);
            }

            // Sorting the list, with last updated post first
            postDTOList.Sort((x, y) => DateTime.Compare(y.LastUpdate, x.LastUpdate));

            return postDTOList;
        }

        /// <summary>
        /// Get all Direct messages to a spesific user 
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns>List of posts</returns>
        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<DirectMessagesDTO>>> GetDirectMessages([FromHeader] Guid user_id)
        {
            var messagesIncoming = await _context.Posts.Where(post => post.TargetUser == user_id).ToListAsync();
            var messagesOutgoing = await _context.Posts.Where(post => post.UserId == user_id).Where(post => post.TargetUser != null).ToListAsync();


            var dmDTOList = new List<DirectMessagesDTO>();

            foreach (var message in messagesIncoming)
            {
                var element = dmDTOList.Find(x => x.SenderID == message.UserId);
                if (element != null)
                {
                    var commentDTO = new CommentDTO();
                    commentDTO.PostId = message.PostId;
                    var _createdBy = await _context.Users.FindAsync(message.UserId);
                    commentDTO.CreatedBy = _createdBy.UserName;
                    commentDTO.Target = message.TargetPost;
                    commentDTO.LastUpdate = message.LastUpdate;
                    commentDTO.Body = message.Body;
                    commentDTO.Title = message.Title;
                    element.Messages.Add(commentDTO);

                }
                else
                {
                    var dmDTO = new DirectMessagesDTO();

                    dmDTO.SenderID = message.UserId;
                    var _createdBy = await _context.Users.FindAsync(message.UserId);
                    dmDTO.Sender = _createdBy.UserName;
                    dmDTO.Image = _createdBy.Image;

                    var commentDTO = new CommentDTO();
                    commentDTO.PostId = message.PostId;
                    commentDTO.CreatedBy = _createdBy.UserName;
                    commentDTO.Target = message.TargetPost;
                    commentDTO.LastUpdate = message.LastUpdate;
                    commentDTO.Body = message.Body;
                    commentDTO.Title = message.Title;
                    dmDTO.Messages.Add(commentDTO);

                    dmDTOList.Add(dmDTO);
                }

            }

            foreach (var message in messagesOutgoing)
            {
                var element = dmDTOList.Find(x => x.SenderID == message.TargetUser);
                if (element != null)
                {
                    var commentDTO = new CommentDTO();
                    commentDTO.PostId = message.PostId;
                    var _createdBy = await _context.Users.FindAsync(message.UserId);
                    commentDTO.CreatedBy = _createdBy.UserName;
                    commentDTO.Target = message.TargetPost;
                    commentDTO.LastUpdate = message.LastUpdate;
                    commentDTO.Body = message.Body;
                    commentDTO.Title = message.Title;
                    element.Messages.Add(commentDTO);

                }
                else
                {
                    var dmDTO = new DirectMessagesDTO();

                    dmDTO.SenderID = message.UserId;
                    var _createdBy = await _context.Users.FindAsync(message.UserId);
                    dmDTO.Sender = _createdBy.UserName;
                    dmDTO.Image = _createdBy.Image;

                    var commentDTO = new CommentDTO();
                    commentDTO.PostId = message.PostId;
                    commentDTO.CreatedBy = _createdBy.UserName;
                    commentDTO.Target = message.TargetPost;
                    commentDTO.LastUpdate = message.LastUpdate;
                    commentDTO.Body = message.Body;
                    commentDTO.Title = message.Title;
                    dmDTO.Messages.Add(commentDTO);

                    dmDTOList.Add(dmDTO);
                }

            }

            if (messagesIncoming == null)
            {
                return NotFound();
            }

            foreach (var dmList in dmDTOList)
            {
                dmList.Messages.Sort((x, y) => DateTime.Compare(y.LastUpdate, x.LastUpdate));
            }



            return dmDTOList;
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
        /// <param name="post_id"></param>
        /// <returns></returns>
        [HttpPatch("{post_id}")]
        public async Task<IActionResult> PatchPost(int post_id, Post post, [FromHeader] Guid user_id)
        {
            Debug.WriteLine("\n\n\nPost: " + post);
            Debug.WriteLine("\n\n\nPostid: " + post_id);
            Debug.WriteLine("\n\n\nUser_id: " + user_id);
            if (post.TargetGroup != null || post.TargetPost != null || post.TargetUser != null || post.TargetEvent != null || post.TargetTopic != null)
            {
                return Forbid();
            }

            // Find existing entity
            var existingEntity = await _context.Posts.FindAsync(post_id);


            // Handle not found
            if (existingEntity == null)
            {
                return NotFound();
            }

            if (existingEntity.UserId != user_id)
            {
                return Forbid();
            }

            // Iterate through all of the properties of the update object
            foreach (PropertyInfo prop in post.GetType().GetProperties())
            {
                // Check if the property has been set in the updateObject
                if (prop.GetValue(post) != null)
                {
                    // If it has been set update the existing entity value
                    existingEntity.GetType().GetProperty(prop.Name)?.SetValue(existingEntity, prop.GetValue(post));
                }
            }
            DateTime now = DateTime.Now;
            existingEntity.LastUpdate = now;

            // Save to DB
            await _context.SaveChangesAsync();
            return NoContent();

        }


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

                EventUser eventUser = new EventUser();
                eventUser.UserId = user.UserId;
                eventUser.User = user;

                exists = _event.Any(_event => _event.Users.Contains(eventUser));
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

            if (post.TargetPost != null)
            {

                // Check if target post exists
                var _post = _context.Posts.Where(p => p.PostId == post.TargetPost);
                if (_post == null)
                {
                    return NotFound();
                }
                else
                {
                    exists = true;
                }
            }

            //if (!exists)
            //{
            //    return Forbid();
            //}

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
