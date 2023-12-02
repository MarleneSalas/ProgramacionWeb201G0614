using U3A1.Models.Entities;

namespace U3A1.Models.ViewModels
{
    public class MenuViewModel
    {
        public IEnumerable<ClasificacionModel> Clasificaciones { get; set; } = null!;  
        public Menu? MenuInfo { get; set; }
    }

    public class ClasificacionModel
    {
        public string Nombre { get; set; } = null!; 
        public IEnumerable<MenuModel> Menus { get; set; } = null!;
    }

    public class MenuModel
    {
        public int IdMenu { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal Precio { get; set; }
    }
}
