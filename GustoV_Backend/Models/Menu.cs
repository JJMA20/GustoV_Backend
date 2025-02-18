using System.ComponentModel.DataAnnotations;

namespace GustoV_Backend.Models
{
    public class Menu
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string nombre { get; set; }

        public decimal precio { get; set; }
        public string descripcion { get; set; }
    }
}
