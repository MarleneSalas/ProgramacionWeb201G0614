namespace U3A1.Models.ViewModels
{
    public class PromocionesViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public decimal Precio { get; set; }
        public double? PrecioPromocion { get; set; }
        public string Anterior { get; set; } = null!;
        public string Siguiente { get; set; } = null!;
    }
}
