using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GustoV_Backend.Controllers;
using GustoV_Backend.Models;
using Xunit;
using GustoV_Backend.Data;

namespace GustoV_Backend.Tests
{
    public class MenuControllerTests
    {
        private readonly DataContex _context;
        private readonly MenuController _controller;

        public MenuControllerTests()
        {
            var options = new DbContextOptionsBuilder<DataContex>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new DataContex(options);

            _context.Menus.AddRange(
                new Menu { id = 1, nombre = "Picante de Pollo", precio = 20 },
                new Menu { id = 2, nombre = "Charque", precio = 25 }
            );
            _context.SaveChanges();

            _controller = new MenuController(_context);
        }

        [Fact]
        public async Task GetMenus_ReturnsListOfMenus()
        {
            var result = await _controller.GetMenus();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var menus = Assert.IsType<List<Menu>>(okResult.Value);

            Assert.Equal(2, menus.Count);
        }
    }
}
