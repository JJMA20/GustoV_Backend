using GustoV_Backend.Data;
using GustoV_Backend.Models;
using GustoV_Backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GustoV_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly DataContex _context;

        public VentaController(DataContex context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Venta>>> GetVentas()
        {
            return await _context.Ventas
                .Include(v => v.detalles)
                    .ThenInclude(d => d.menu)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Venta>> GetVenta(int id)
        {
            var venta = await _context.Ventas
                .Include(v => v.detalles)
                    .ThenInclude(d => d.menu)
                .FirstOrDefaultAsync(v => v.id == id);

            if (venta == null)
            {
                return NotFound();
            }

            return venta;
        }

        [HttpPost]
        public async Task<ActionResult<Venta>> CrearVenta([FromBody] VentaCreateDTO ventaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var venta = new Venta
            {
                fecha = DateTime.Now,
                metodoPago = ventaDto.metodoPago,
                total = 0,
                detalles = new List<DetalleVenta>()
            };

            foreach (var detalleDto in ventaDto.detalles)
            {
                var menu = await _context.Menus.FindAsync(detalleDto.menuId);
                if (menu == null)
                {
                    return BadRequest($"Menú con ID {detalleDto.menuId} no encontrado");
                }

                var detalle = new DetalleVenta
                {
                    menuId = detalleDto.menuId,
                    cantidad = detalleDto.cantidad,
                    subtotal = menu.precio * detalleDto.cantidad
                };

                venta.detalles.Add(detalle);
                venta.total += detalle.subtotal;
            }

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVenta), new { id = venta.id }, venta);
        }

        [HttpPost("{ventaId}/detalles")]
        public async Task<ActionResult<DetalleVenta>> AgregarDetalle(int ventaId, [FromBody] VentaDetalleDTO detalleDto)
        {
            var venta = await _context.Ventas
                .Include(v => v.detalles)
                .FirstOrDefaultAsync(v => v.id == ventaId);

            if (venta == null)
            {
                return NotFound("Venta no encontrada");
            }

            var menu = await _context.Menus.FindAsync(detalleDto.menuId);
            if (menu == null)
            {
                return BadRequest("Menú no encontrado");
            }

            var detalle = new DetalleVenta
            {
                ventaId = ventaId,
                menuId = detalleDto.menuId,
                cantidad = detalleDto.cantidad,
                subtotal = menu.precio * detalleDto.cantidad
            };

            venta.detalles.Add(detalle);
            venta.total += detalle.subtotal;

            await _context.SaveChangesAsync();

            return Ok(detalle);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarVenta(int id)
        {
            var venta = await _context.Ventas
                .Include(v => v.detalles)
                .FirstOrDefaultAsync(v => v.id == id);

            if (venta == null)
            {
                return NotFound();
            }

            _context.Ventas.Remove(venta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{ventaId}/detalles/{detalleId}")]
        public async Task<IActionResult> EliminarDetalle(int ventaId, int detalleId)
        {
            var venta = await _context.Ventas
                .Include(v => v.detalles)
                .FirstOrDefaultAsync(v => v.id == ventaId);

            if (venta == null)
            {
                return NotFound("Venta no encontrada");
            }

            var detalle = venta.detalles.FirstOrDefault(d => d.id == detalleId);
            if (detalle == null)
            {
                return NotFound("Detalle no encontrado");
            }

            venta.total -= detalle.subtotal;
            _context.DetallesVenta.Remove(detalle);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}