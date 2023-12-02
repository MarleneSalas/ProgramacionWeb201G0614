using U3A1.Models.Entities;

namespace U3A1.Areas.Admin.Models
{
    public class AdminMenuViewModel
    {
        public string Nombre { get; set; } = null!;
        public IEnumerable<MenuModel> Menus { get; set; } = null!;
    }

    public class MenuModel
    {
        public int IdMenu { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal Precio { get; set; }
        public double? PrecioPromocion { get; set; }
        public string Descripcion { get; set; } = null!;
    }
}
