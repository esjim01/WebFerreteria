﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebFerreteria.Models;

namespace WebFerreteria.Controllers
{
    public class ProductoController : Controller
    {
        private readonly FerreteriaDbContext _context;

        public ProductoController(FerreteriaDbContext context)
        {
            _context = context;
        }

        // GET: Producto
        public async Task<IActionResult> Index()
        {
            var productos = await _context.Productos
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            return View(productos);
        }

        // GET: Producto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.Id == id);

            return producto == null ? NotFound() : View(producto);
        }

        // GET: Producto/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Producto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Precio,Cantidad")] Productos productos)
        {
            // Validaciones adicionales
            if (productos.Precio <= 0)
                ModelState.AddModelError("Precio", "El precio debe ser mayor que cero");

            if (productos.Cantidad < 0)
                ModelState.AddModelError("Cantidad", "La cantidad no puede ser negativa");

            if (ModelState.IsValid)
            {
                _context.Add(productos);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Producto creado correctamente";
                return RedirectToAction(nameof(Index));
            }
            return View(productos);
        }

        // GET: Producto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productos = await _context.Productos.FindAsync(id);
            if (productos == null)
            {
                return NotFound();
            }
            return View(productos);
        }

        // POST: Producto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Precio,Cantidad")] Productos productos)
        {
            if (id != productos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productos);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Producto actualizado correctamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductosExists(productos.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(productos);
        }

        // GET: Producto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productos = await _context.Productos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productos == null)
            {
                return NotFound();
            }

            return View(productos);
        }

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productos = await _context.Productos.FindAsync(id);
            {
                try
                {
                    _context.Productos.Remove(productos);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Producto eliminado correctamente";
                }
                catch (DbUpdateException)
                {
                    TempData["ErrorMessage"] = "No se pudo eliminar el producto. Posiblemente esté siendo usado en ventas.";
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // Método para vender producto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VenderProducto(int id, int cantidad)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                TempData["ErrorMessage"] = "Producto no encontrado";
                return RedirectToAction(nameof(Index));
            }

            if (cantidad <= 0)
            {
                TempData["ErrorMessage"] = "La cantidad debe ser mayor que cero";
                return RedirectToAction("Details", new { id });
            }

            if (producto.Cantidad < cantidad)
            {
                TempData["ErrorMessage"] = $"Stock insuficiente. Solo quedan {producto.Cantidad} unidades";
                return RedirectToAction("Details", new { id });
            }

            producto.Cantidad -= cantidad;
            _context.Update(producto);

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Venta registrada. Stock actual: {producto.Cantidad}";
            }
            catch (DbUpdateException ex)
            {
                TempData["ErrorMessage"] = "Error al registrar la venta: " + ex.Message;
            }

            return RedirectToAction("Details", new { id });
        }

        private bool ProductosExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}
