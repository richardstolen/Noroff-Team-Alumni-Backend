﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// Get all groups 
        /// </summary>
        /// <returns>List of groups</returns>
        [HttpGet("/groups")]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroups()
        {
            return await _context.Groups.ToListAsync();
        }

        /// <summary>
        /// Get a spesific group by id
        /// </summary>
        /// <param name="group_id"></param>
        /// <returns>The choosen group</returns>
        [HttpGet("/group/{group_id}")]
        public async Task<ActionResult<Group>> GetGroup(int group_id)
        {
            var group = await _context.Groups.FindAsync(group_id);

            if (group == null)
            {
                return NotFound();
            }

            return group;
        }

        /// <summary>
        /// Update a spesific group
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
        /// Create new groups
        /// </summary>
        /// <param name="group"></param>
        /// <returns>New group</returns>
        [HttpPost("/group")]
        [ActionName(nameof(GetGroup))]
        public async Task<ActionResult<Group>> PostGroup([FromBody] Group @group)
        {
            _context.Groups.Add(@group);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGroup), new { id = @group.GroupId }, @group);
        }

        /// <summary>
        /// Delete a group
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

        /// <summary>
        /// Add User to Group. If private group, needs Admin_id to be equal
        /// to the user to who created the group.
        /// </summary>
        /// <param name="group_id"></param>
        /// <param name="user_id"></param>
        /// <param name="admin_id"></param>
        /// <returns></returns>
        [HttpPost("/group/{group_id}/join")]
        public async Task<IActionResult> AddUserToGroup(int group_id, [FromHeader] Guid user_id, [FromHeader] Guid admin_id)
        {
            Debug.WriteLine("\nGroup id: " + group_id);
            Debug.WriteLine("\nUser id: " + user_id);
            Debug.WriteLine("\nAdmin id: " + admin_id);
            // TODO ADD TO PRIVATE GROUP WITH ADMIN USER

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
        /// Remove user from Group. If private group, needs Admin_id to be equal
        /// to the user to who created the group.
        /// </summary>
        /// <param name="group_id"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        [HttpPost("/group/{group_id}/remove")]
        public async Task<IActionResult> RemoveUserFromGroup(int group_id, [FromHeader] Guid user_id)
        {
            // Load the parent entity that contains the collection of child entities
            var parentEntity = _context.Groups.Include(p => p.Users).FirstOrDefault(p => p.GroupId == group_id);

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
