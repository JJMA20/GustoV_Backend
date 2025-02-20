using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GustoV_Backend.DTOs
{
    public class VentaCreateDTO
    {
        [Required]
        public string metodoPago { get; set; }
        public List<DetalleVentaDto> detalles { get; set; } = new List<DetalleVentaDto>();
    }
}
