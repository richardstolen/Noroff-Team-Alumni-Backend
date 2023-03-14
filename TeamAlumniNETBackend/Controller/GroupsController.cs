using System;
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
    public class GroupsController : ControllerBase
    {
        private readonly AlumniDbContext _context;

        public GroupsController(AlumniDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all groups 
        /// </summary>
        /// <returns>List of groups</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroups()
        {
            return await _context.Groups.ToListAsync();
        }

        /// <summary>
        /// Get a spesific group by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The choosen group</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> GetGroup(int id)
        {
            var @group = await _context.Groups.FindAsync(id);

            if (@group == null)
            {
                return NotFound();
            }

            return @group;
        }

        /// <summary>
        /// Update a spesific group
        /// </summary>
        /// <param name="id"></param>
        /// <param name="group"></param>
        /// <returns>Updated group</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroup(int id, Group @group)
        {
            if (id != @group.GroupId)
            {
                return BadRequest();
            }

            _context.Entry(@group).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(id))
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
        /// Create new groups
        /// </summary>
        /// <param name="group"></param>
        /// <returns>New group</returns>
        [HttpPost]
        public async Task<ActionResult<Group>> PostGroup(Group @group)
        {
            _context.Groups.Add(@group);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroup", new { id = @group.GroupId }, @group);
        }

        /// <summary>
        /// Delete a group
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Deleted group</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var @group = await _context.Groups.FindAsync(id);
            if (@group == null)
            {
                return NotFound();
            }

            _context.Groups.Remove(@group);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPost("{id}/join")]
        public async Task<IActionResult> AddUserToGroup(int id, [FromBody] int user_id, [FromHeader] int admin_id)
        {
            Debug.WriteLine("Group id: " + id);
            Debug.WriteLine("User id: " + user_id);
            Debug.WriteLine("Admin id: " + admin_id);
            //int user_id = 1;

            //Get user and Group
            var user = await _context.Users.FindAsync(user_id);
            var group = await _context.Groups.FindAsync(id);

            // Handle Not Found
            if (group == null || user == null)
            {
                return NotFound();
            }
            // Add User to Topic
            group.Users.Add(user);

            // Save
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GroupExists(int id)
        {
            return _context.Groups.Any(e => e.GroupId == id);
        }

    }
}
