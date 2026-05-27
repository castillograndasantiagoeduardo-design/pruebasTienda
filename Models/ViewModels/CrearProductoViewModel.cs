using System.ComponentModel.DataAnnotations;

namespace PruebaBackendTienda.Models.ViewModels
{
    public class CrearProductoViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, 999999, ErrorMessage = "El precio debe ser mayor a $0")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio")]
        [Range(0, 9999, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Selecciona una categoría")]
        public int Id_Categoria { get; set; }

        [StringLength(300)]
        public string? Descripcion { get; set; }

        // El archivo de imagen — se guarda en wwwroot/images/productos/
        public IFormFile? Imagen { get; set; }
    }
}
