namespace PruebaBackendTienda.Models.Domain
{
    public class Categoria
    {
        /// <summary>
        /// Mapea la tabla [Categoria] de la base de datos SenaPay.
        /// </summary>
            public int Id_Categoria { get; set; }
            public string Nombre_Categoria { get; set; } = string.Empty;

            // Propiedad de navegación: una Categoria tiene muchos Productos
            public ICollection<Producto> Productos { get; set; } = new List<Producto>();
        
    }
}
