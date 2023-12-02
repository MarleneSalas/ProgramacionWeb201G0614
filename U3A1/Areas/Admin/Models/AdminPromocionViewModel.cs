namespace U3A1.Areas.Admin.Models
{
    public class AdminPromocionViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal Precio { get; set; }
        public double? PrecioPromocion { get; set; }
    }
}
