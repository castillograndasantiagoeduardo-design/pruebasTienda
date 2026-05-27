using System.ComponentModel.DataAnnotations;

namespace PruebaBackendTienda.Models.ViewModels
{
    public class EditarStockViewModel
    {
        public int Id_Producto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public int StockActual { get; set; }

        // "Sumar", "Restar", "Fijar"
        [Required(ErrorMessage = "Selecciona una operación")]
        public string Operacion { get; set; } = "Sumar";

        [Required(ErrorMessage = "Ingresa una cantidad")]
        [Range(1, 9999, ErrorMessage = "La cantidad debe ser entre 1 y 9999")]
        public int Cantidad { get; set; }
    }
}
