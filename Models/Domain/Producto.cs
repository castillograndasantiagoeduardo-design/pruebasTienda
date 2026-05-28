using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaBackendTienda.Models.Domain
{
    public class Producto
    {
        [Key]                                    // ← agrega esto
        public int Id_Producto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? ImagenUrl { get; set; }
        public bool Estado { get; set; }
        public bool Activo { get; set; } = true;
        public string? Descripcion { get; set; }

        public int Id_Tienda { get; set; }
        [ForeignKey("Id_Tienda")]
        public Tienda Tienda { get; set; }

        public int Id_Categoria { get; set; }
        [ForeignKey("Id_Categoria")]
        public Categoria Categoria { get; set; }
    }
}