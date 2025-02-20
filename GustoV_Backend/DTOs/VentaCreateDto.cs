using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GustoV_Backend.DTOs
{
    public class VentaCreateDTO
    {
        [Required]
        public string metodoPago { get; set; }
        public List<VentaDetalleDTO> detalles { get; set; } = new List<VentaDetalleDTO>();
    }
}
