using GustoV_Backend.Data;
using GustoV_Backend.Models;
using GustoV_Backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;

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


        [HttpGet("reporte-diario")]
        public async Task<ActionResult> GetReporteVentasDiario([FromQuery] DateTime? fecha = null)
        {
            var fechaConsulta = fecha ?? DateTime.UtcNow.Date;

            var ventasDelDia = await _context.Ventas
                .Where(v => v.fecha.Date == fechaConsulta)
                .Include(v => v.detalles)
                .ThenInclude(d => d.menu)
                .ToListAsync();

            if (ventasDelDia.Count == 0)
            {
                return NotFound(new { mensaje = "No hay ventas registradas para la fecha indicada." });
            }

            var totalVentas = ventasDelDia.Count;
            var totalIngresos = ventasDelDia.Sum(v => v.total);
            var platosVendidos = ventasDelDia
                .SelectMany(v => v.detalles)
                .GroupBy(d => d.menu.nombre)
                .Select(g => new { Plato = g.Key, Cantidad = g.Sum(d => d.cantidad) })
                .ToList();

            return Ok(new
            {
                Fecha = fechaConsulta,
                TotalVentas = totalVentas,
                TotalIngresos = totalIngresos,
                PlatosVendidos = platosVendidos
            });
        }

        [HttpGet("reporte-diario/excel")]
        public async Task<IActionResult> GetReporteVentasDiarioExcel([FromQuery] DateTime? fecha = null)
        {
            var fechaConsulta = fecha ?? DateTime.UtcNow.Date;

            var ventasDelDia = await _context.Ventas
                .Where(v => v.fecha.Date == fechaConsulta)
                .Include(v => v.detalles)
                .ThenInclude(d => d.menu)
                .ToListAsync();

            if (ventasDelDia.Count == 0)
            {
                return NotFound(new { mensaje = "No hay ventas registradas para la fecha indicada." });
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Reporte Diario");

                worksheet.Cell(1, 1).Value = "Fecha";
                worksheet.Cell(1, 2).Value = "Plato";
                worksheet.Cell(1, 3).Value = "Cantidad Vendida";
                worksheet.Cell(1, 4).Value = "Total Venta (Bs.)";

                int row = 2;
                foreach (var venta in ventasDelDia)
                {
                    foreach (var detalle in venta.detalles)
                    {
                        worksheet.Cell(row, 1).Value = venta.fecha.ToString("yyyy-MM-dd");
                        worksheet.Cell(row, 2).Value = detalle.menu.nombre;
                        worksheet.Cell(row, 3).Value = detalle.cantidad;
                        worksheet.Cell(row, 4).Value = detalle.cantidad * detalle.menu.precio;
                        row++;
                    }
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Reporte_Ventas_{fechaConsulta:yyyyMMdd}.xlsx");
                }
            }
        }

    }
}