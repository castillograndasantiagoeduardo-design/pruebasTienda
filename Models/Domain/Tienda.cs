namespace PruebaBackendTienda.Models.Domain
{
    /// <summary>
    /// Mapea la tabla [Tienda] de la base de datos SenaPay.
    /// </summary>
    public class Tienda
    {
        public int Id_Tienda { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Ubicacion { get; set; }
        public int Id_Admin_Cafeteria { get; set; }

        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }

}
