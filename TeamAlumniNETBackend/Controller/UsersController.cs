﻿using System;
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
using TeamAlumniNETBackend.DTOs.UserDTOs;
using TeamAlumniNETBackend.Models;

namespace TeamAlumniNETBackend.Controller
{
    [Authorize]
    [Route("")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AlumniDbContext _context;

        public UsersController(AlumniDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all Users.
        /// </summary>
        /// <returns></returns>
        [HttpGet("/users")]

        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Get User by ID.
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        [HttpGet("user/{user_id}")]
        public async Task<ActionResult<UserGetDTO>> GetUserById(Guid user_id)
        {
            var user = await _context.Users.FindAsync(user_id);

            if (user == null)
            {
                return NotFound();
            }

            EventUser eventUser = new EventUser();
            eventUser.UserId = user.UserId;
            eventUser.User = user;

            var groups = await _context.Groups.Where(g => g.Users.Contains(user)).ToListAsync();
            var topics = await _context.Topics.Where(t => t.Users.Contains(user)).ToListAsync();
            var events = await _context.Events.Where(e => e.Users.Contains(eventUser)).ToListAsync();

            UserGetDTO userDTO = new UserGetDTO();

            userDTO.UserId = user.UserId;
            userDTO.UserName = user.UserName;
            userDTO.Status = user.Status;
            userDTO.Bio = user.Bio;
            userDTO.Image = user.Image;
            userDTO.FunFact = user.FunFact;
            userDTO.Events = events;
            userDTO.Groups = groups;
            userDTO.Topics = topics;

            return userDTO;
        }

        /// <summary>
        /// Get User by username. Usernames are unique.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>200: User object <br/>404: Not Found </returns>
        [HttpGet("user/username/{username}")]
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
        /// <param name="user_id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPatch("/user/{user_id}")]
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

        /// <summary>
        /// Create new user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("/user")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        /// <summary>
        /// Delete User by id.
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Check if user exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A bool</returns>
        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
