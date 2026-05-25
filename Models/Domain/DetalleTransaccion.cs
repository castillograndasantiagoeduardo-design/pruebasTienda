namespace PruebaBackendTienda.Models.Domain
{
    /// <summary>
    /// Mapea la tabla [Detalle_Transaccion]. Cada línea de producto dentro de un ticket.
    /// </summary>
    public class DetalleTransaccion
    {
        public int Id_Detalle { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio_Unitario { get; set; }  // Guardamos el precio AL MOMENTO de la compra
        public int Id_Transaccion { get; set; }
        public int Id_Producto { get; set; }

        public Transaccion? Transaccion { get; set; }
        public Producto? Producto { get; set; }
    }

}
