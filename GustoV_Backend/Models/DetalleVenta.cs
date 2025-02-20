// Models/DetalleVenta.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GustoV_Backend.Models
{
    public class DetalleVenta
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int ventaId { get; set; }

        [ForeignKey("ventaId")]
        public virtual Venta venta { get; set; }

        [Required]
        public int menuId { get; set; }

        [ForeignKey("menuId")]
        public virtual Menu menu { get; set; }

        [Required]
        public int cantidad { get; set; }

        public decimal subtotal { get; set; }
    }
}