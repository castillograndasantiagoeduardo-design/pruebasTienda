namespace PruebaBackendTienda.Models.Domain
{
    /// <summary>
    /// Mapea la tabla [Productos] de la base de datos SenaPay.
    /// </summary>
    public class Producto
    {
        public int Id_Producto { get; set; }
        public string Nombre_Producto { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? Imagen { get; set; }         // Nullable: puede no tener imagen
        public bool Estado { get; set; }             // true = activo, false = inactivo
        public int Id_Tienda { get; set; }
        public int Id_Categoria { get; set; }

        // Propiedades de navegación (EF Core las rellena automáticamente con .Include())
        public Tienda? Tienda { get; set; }
        public Categoria? Categoria { get; set; }
    }

}
