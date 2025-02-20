using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using GustoV_Backend.Controllers;
using GustoV_Backend.Models;
using GustoV_Backend.DTOs;
using GustoV_Backend.Data;

namespace GustovAPI.Tests
{
    public class VentasControllerTests
    {
        private readonly DataContex _context;
        private readonly VentaController _controller;

        public VentasControllerTests()
        {
            var options = new DbContextOptionsBuilder<DataContex>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new DataContex(options);
            _controller = new VentaController(_context);

            _context.Menus.AddRange(
                new Menu { id = 1, nombre = "Picante de Pollo", precio = 20 },
                new Menu { id = 2, nombre = "Charque", precio = 25 }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task PostVenta_CalculatesTotalCorrectly()
        {
            var ventaDto = new VentaCreateDTO
            {
                metodoPago = "Efectivo",
                detalles = new List<DetalleVentaDto>
                {
                    new DetalleVentaDto { menuId = 1, cantidad = 2 },
                    new DetalleVentaDto { menuId = 2, cantidad = 1 }
                }
            };

            var result = await _controller.CrearVenta(ventaDto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);

            var createdVenta = Assert.IsType<Venta>(createdResult.Value);

            Assert.Equal(65, createdVenta.total);
        }
    }
}
