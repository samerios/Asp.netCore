using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.Models;

namespace project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VegetablesController : ControllerBase
    {
        private readonly DBvegetableContext _context;

        public VegetablesController(DBvegetableContext context)
        {
            _context = context;
        }

        // GET API - Get All Vegetables(Allow access for non-registered users)
        // GET: api/Vegetables/GetAllVegetables
        [HttpGet("GetAllVegetables")]
        public async Task<ActionResult<IEnumerable<Vegetable>>> GetVegetables()
        {
            return await _context.Vegetables.ToListAsync();
        }

        // GET API - Get All Small Vegetables (Allow access for non-registered users)
        // GET: api/Vegetables/GetAllSmallVegetables
        [HttpGet("GetAllSmallVegetables")]
        public async Task<ActionResult<IEnumerable<Vegetable>>> GetSmallVegetables()
        {
            return await _context.Vegetables.Where(vegetables => vegetables.Size == "Small").ToListAsync();
        }

        // PUT API - Update Vegetable (Only registered users)
        // PUT: api/Vegetables/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVegetable(string id, Vegetable vegetable)
        {
            if (id != vegetable.Name)
            {
                return BadRequest();
            }

            _context.Entry(vegetable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VegetableExists(id))
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

        // POST API - Create Vegetable (Only registered users)
        // POST: api/createVegetable/
        [Authorize]
        [HttpPost("createVegetable")]
        public async Task<ActionResult<Vegetable>> PostVegetable(Vegetable vegetable)
        {
            _context.Vegetables.Add(vegetable);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (VegetableExists(vegetable.Name))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetVegetable", new { id = vegetable.Name }, vegetable);
        }

        // DELETE API - Delete Single Vegetable.  (Only registered users)
        // DELETE: api/Vegetables/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVegetable(string id)
        {
            var vegetable = await _context.Vegetables.FindAsync(id);
            if (vegetable == null)
            {
                return NotFound();
            }

            _context.Vegetables.Remove(vegetable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Check if Vegetable exists
        private bool VegetableExists(string id)
        {
            return _context.Vegetables.Any(e => e.Name == id);
        }
    }
}
