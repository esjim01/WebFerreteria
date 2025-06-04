using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiFerreteria.Models;

namespace WebApiFerreteria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntregaController : ControllerBase
    {
        private readonly FerreteriaDbContext _context;

        public EntregaController(FerreteriaDbContext context)
        {
            _context = context;
        }

        // GET: api/Entrega
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entregas>>> GetEntregas()
        {
            return await _context.Entregas.ToListAsync();
        }

        // GET: api/Entrega/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Entregas>> GetEntregas(int id)
        {
            var entregas = await _context.Entregas.FindAsync(id);

            if (entregas == null)
            {
                return NotFound();
            }

            return entregas;
        }

        // PUT: api/Entrega/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEntregas(int id, Entregas entregas)
        {
            if (id != entregas.Id)
            {
                return BadRequest();
            }

            _context.Entry(entregas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntregasExists(id))
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

        // POST: api/Entrega
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Entregas>> PostEntregas(Entregas entregas)
        {
            _context.Entregas.Add(entregas);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEntregas", new { id = entregas.Id }, entregas);
        }

        // DELETE: api/Entrega/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntregas(int id)
        {
            var entregas = await _context.Entregas.FindAsync(id);
            if (entregas == null)
            {
                return NotFound();
            }

            _context.Entregas.Remove(entregas);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EntregasExists(int id)
        {
            return _context.Entregas.Any(e => e.Id == id);
        }
    }
}
