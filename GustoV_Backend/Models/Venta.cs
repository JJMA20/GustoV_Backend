// Models/Venta.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace GustoV_Backend.Models
{
    public class Venta
    {
        [Key]
        public int id { get; set; }

        [Required]
        public DateTime fecha { get; set; }

        [Required]
        public decimal total { get; set; }

        [Required]
        public string metodoPago { get; set; }

        // Relación con los detalles
        public virtual ICollection<DetalleVenta> detalles { get; set; }
    }
}