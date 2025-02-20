using GustoV_Backend.Data;
using GustoV_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GustoV_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleVentaController : ControllerBase
    {
        //private readonly DataContex _context;

        //public DetalleVentaController(DataContex context)
        //{
        //    _context = context;
        //}

        //[HttpGet("venta/{ventaId}")]
        //public async Task<ActionResult<IEnumerable<DetalleVenta>>> GetDetallesPorVenta(int ventaId)
        //{
        //    return await _context.DetallesVenta
        //        .Include(d => d.menu)
        //        .Where(d => d.ventaId == ventaId)
        //        .ToListAsync();
        //}

        //[HttpPost]
        //public async Task<ActionResult<DetalleVenta>> CrearDetalle([FromBody] DetalleVenta detalle)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var menu = await _context.Menus.FindAsync(detalle.menuId);
        //    if (menu == null)
        //    {
        //        return BadRequest("Menú no encontrado");
        //    }

        //    var venta = await _context.Ventas.FindAsync(detalle.ventaId);
        //    if (venta == null)
        //    {
        //        return BadRequest("Venta no encontrada");
        //    }

        //    detalle.subtotal = menu.precio * detalle.cantidad;

        //    _context.DetallesVenta.Add(detalle);
        //    await _context.SaveChangesAsync();

        //    venta = await _context.Ventas
        //        .Include(v => v.detalles)
        //        .FirstOrDefaultAsync(v => v.id == detalle.ventaId);

        //    if (venta != null)
        //    {
        //        venta.total = venta.detalles.Sum(d => d.subtotal);
        //        await _context.SaveChangesAsync();
        //    }

        //    return CreatedAtAction(nameof(GetDetallesPorVenta),
        //        new { ventaId = detalle.ventaId },
        //        detalle);
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> EliminarDetalle(int id)
        //{
        //    var detalle = await _context.DetallesVenta.FindAsync(id);
        //    if (detalle == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.DetallesVenta.Remove(detalle);
        //    await _context.SaveChangesAsync();

        //    var venta = await _context.Ventas
        //        .Include(v => v.detalles)
        //        .FirstOrDefaultAsync(v => v.id == detalle.ventaId);

        //    if (venta != null)
        //    {
        //        venta.total = venta.detalles.Sum(d => d.subtotal);
        //        await _context.SaveChangesAsync();
        //    }

        //    return NoContent();
        //}
    }
}