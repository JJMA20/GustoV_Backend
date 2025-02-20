using System.ComponentModel.DataAnnotations;

public class DetalleVentaDto
{
    [Required]
    public int menuId { get; set; }
    [Required]
    public int cantidad { get; set; }
}