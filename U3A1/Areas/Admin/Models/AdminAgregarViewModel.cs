using U3A1.Models.Entities;
using U3A1.Models.ViewModels;

namespace U3A1.Areas.Admin.Models
{
	public class AdminAgregarViewModel
	{
		public IEnumerable<ClasificacionModel> Clasificaciones { get; set; } = null!;
		public IFormFile? Archivo { get; set; }
		public Menu MenuN { get; set; } = new();
	}

	public class ClasificacionModel
	{
        public int Id { get; set; }
        public string Nombre { get; set; }=null!;
    }
}
