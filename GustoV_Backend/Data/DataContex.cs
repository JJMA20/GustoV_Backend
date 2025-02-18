using GustoV_Backend.Controllers;
using GustoV_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace GustoV_Backend.Data
{
    public class DataContex : DbContext
    {

        public DataContex(DbContextOptions<DataContex> options) : base(options) { }

        public DbSet<Menu> Menus { get; set; }
    }
}
