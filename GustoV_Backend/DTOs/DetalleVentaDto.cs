using System.ComponentModel.DataAnnotations;

public class VentaDetalleDTO
{
    [Required]
    public int menuId { get; set; }
    [Required]
    public int cantidad { get; set; }
}