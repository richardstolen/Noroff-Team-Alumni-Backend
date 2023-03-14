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
    [Route("")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly AlumniDbContext _context;

        public GroupsController(AlumniDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// methode to get all groups 
        /// </summary>
        /// <returns>List of groups</returns>
        [HttpGet("/groups")]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroups()
        {
            return await _context.Groups.ToListAsync();
        }

        /// <summary>
        /// Methode to get a spesific group by id
        /// </summary>
        /// <param name="group_id"></param>
        /// <returns>The choosen group</returns>
        [HttpGet("/group/{group_id}")]
        public async Task<ActionResult<Group>> GetGroup(int group_id)
        {
            var @group = await _context.Groups.FindAsync(group_id);

            if (@group == null)
            {
                return NotFound();
            }

            return @group;
        }

        /// <summary>
        /// Methode to update a spesific group
        /// </summary>
        /// <param name="group_id"></param>
        /// <param name="group"></param>
        /// <returns>Updated group</returns>
        [HttpPut("/group/{group_id}")]
        public async Task<IActionResult> PutGroup(int group_id, Group @group)
        {
            if (group_id != @group.GroupId)
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
                if (!GroupExists(group_id))
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
        /// Methode to create new groups
        /// </summary>
        /// <param name="group"></param>
        /// <returns>New group</returns>
        [HttpPost("/group")]
        public async Task<ActionResult<Group>> PostGroup(Group @group)
        {
            _context.Groups.Add(@group);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroup", new { id = @group.GroupId }, @group);
        }

        /// <summary>
        /// Methode to delete a group
        /// </summary>
        /// <param name="group_id"></param>
        /// <returns>Deleted group</returns>
        [HttpDelete("/group/{group_id}")]
        public async Task<IActionResult> DeleteGroup(int group_id)
        {
            var group = await _context.Groups.FindAsync(group_id);
            if (group == null)
            {
                return NotFound();
            }

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPost("/group/{group_id}/join")]
        public async Task<IActionResult> AddUserToGroup(int group_id, [FromBody] int user_id, [FromHeader] int admin_id)
        {
            Debug.WriteLine("Group id: " + group_id);
            Debug.WriteLine("User id: " + user_id);
            Debug.WriteLine("Admin id: " + admin_id);
            //int user_id = 1;

            //Get user and Group
            var user = await _context.Users.FindAsync(user_id);
            var group = await _context.Groups.FindAsync(group_id);

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

        /// <summary>
        /// Check if group exists in DB.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A boolean.</returns>
        private bool GroupExists(int id)
        {
            return _context.Groups.Any(e => e.GroupId == id);
        }

    }
}
