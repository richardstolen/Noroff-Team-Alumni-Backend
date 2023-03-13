using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamAlumniNETBackend.Data;
using TeamAlumniNETBackend.Models;

namespace TeamAlumniNETBackend.Controller
{
    [Route("")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AlumniDbContext _context;

        public UsersController(AlumniDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet("/user")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }


        [HttpGet("user/{user_id}")]  // GET: api/Users/id/{id}
        public async Task<ActionResult<User>> GetUserById(Guid user_id)
        {
            var user = await _context.Users.FindAsync(user_id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        /// <summary>
        /// Get User by username. Usernames are unique.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>200: User object <br/>404: Not Found </returns>
        [HttpGet("user/{username}")]  // GET: api/Users/username/{username}
        public async Task<ActionResult<User>> GetUserByUsername(string username)
        {
            var user = await _context.Users.Where(user => user.UserName == username).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        /// <summary>
        /// Update specific properties on User object
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPatch("/user/{user_id}")] // PATCH: api/Users/id
        public async Task<IActionResult> PatchUser(Guid user_id, User user)
        {
            // Find existing entity
            var existingEntity = await _context.Users.FindAsync(user_id);

            // Handle not found
            if (existingEntity == null)
            {
                return NotFound();
            }

            // Iterate through all of the properties of the update object
            foreach (PropertyInfo prop in user.GetType().GetProperties())
            {
                // Check if the property has been set in the updateObject
                if (prop.GetValue(user) != null)
                {
                    // If it has been set update the existing entity value
                    existingEntity.GetType().GetProperty(prop.Name)?.SetValue(existingEntity, prop.GetValue(user));
                }
            }

            // Save to DB
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("/user")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("/user/{user_id}")]
        public async Task<IActionResult> DeleteUser(Guid user_id)
        {
            var user = await _context.Users.FindAsync(user_id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
