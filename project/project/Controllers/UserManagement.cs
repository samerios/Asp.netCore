using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.Models;

namespace project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagement : ControllerBase
    {
        private readonly DBvegetableContext _context;

        public UserManagement(DBvegetableContext context)
        {
            _context = context;
        }

        // User Login
        // GET: api/UserManagement/Login
        [Authorize]
        [HttpGet("Login")]
        public async Task<ActionResult<User>> GetUser()
        {
            string username = HttpContext.User.Identity.Name;
            var user = await _context.Users.
                Where(user => user.Username == username).FirstOrDefaultAsync();
            user.Password = null;
            if (user == null)
                return NotFound();
            return user;
        }

        // User Register
        // POST: api/UserManagement/Register
        [HttpPost("Register")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.Username))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUser", new { id = user.Username }, user);
        }

        // Check if User exists
        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Username == id);
        }
    }
}
