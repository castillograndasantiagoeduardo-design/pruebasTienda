namespace PruebaBackendTienda.Models.ViewModels
{
    // ViewModel de solo lectura para mostrar productos en el panel admin
    public class AdminProductoViewModel
    {
        public int Id_Producto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? NombreCategoria { get; set; }
        public string? Descripcion { get; set; }
        public string? ImagenUrl { get; set; }
        public bool Activo { get; set; }

        // Helper para la vista
        public string BadgeStock => Stock <= 0 ? "danger" : Stock <= 5 ? "warning" : "success";
        public string TextoStock => Stock <= 0 ? "Sin stock" : Stock <= 5 ? "Stock bajo" : "Disponible";
    }
}
