namespace PruebaBackendTienda.Models.ViewModels
{
    /// <summary>
    /// ViewModel del carrito de compras. NO se guarda en BD, solo vive en sesión.
    /// Separa los datos de presentación de los datos de dominio.
    /// </summary>
    public class CarritoViewModel
    {
        public List<ItemCarrito> Items { get; set; } = new List<ItemCarrito>();
        public decimal Total => Items.Sum(i => i.Subtotal);
        public int TotalItems => Items.Sum(i => i.Cantidad);
    }

    public class ItemCarrito
    {
        public int Id_Producto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal => PrecioUnitario * Cantidad;
    }
}
