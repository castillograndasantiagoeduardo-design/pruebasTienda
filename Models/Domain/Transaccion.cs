namespace PruebaBackendTienda.Models.Domain
{
    /// <summary>
    /// Mapea la tabla [Transacciones]. Representa el "ticket" de una compra completa.
    /// </summary>
    public class Transaccion
    {
        public int Id_Transaccion { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }
        public int Id_Aprendiz { get; set; }

        public Aprendiz? Aprendiz { get; set; }
        public ICollection<DetalleTransaccion> Detalles { get; set; } = new List<DetalleTransaccion>();
    }

}
