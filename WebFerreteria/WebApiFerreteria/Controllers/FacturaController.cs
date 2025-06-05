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
    public class FacturaController : ControllerBase
    {
        private readonly FerreteriaDbContext _context;

        public FacturaController(FerreteriaDbContext context)
        {
            _context = context;
        }

        // GET: api/Factura
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Facturas>>> GetFacturas()
        {
            return await _context.Facturas.ToListAsync();
        }

        // GET: api/Factura/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Facturas>> GetFacturas(int id)
        {
            var facturas = await _context.Facturas.FindAsync(id);

            if (facturas == null)
            {
                return NotFound();
            }

            return facturas;
        }

        // PUT: api/Factura/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFacturas(int id, Facturas facturas)
        {
            if (id != facturas.Id)
            {
                return BadRequest();
            }

            _context.Entry(facturas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacturasExists(id))
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

        // POST: api/Factura
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Facturas>> PostFacturas(Facturas facturas)
        {
            _context.Facturas.Add(facturas);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFacturas", new { id = facturas.Id }, facturas);
        }

        // DELETE: api/Factura/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFacturas(int id)
        {
            var facturas = await _context.Facturas.FindAsync(id);
            if (facturas == null)
            {
                return NotFound();
            }

            _context.Facturas.Remove(facturas);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FacturasExists(int id)
        {
            return _context.Facturas.Any(e => e.Id == id);
        }
    }
}
