using System.Security.Policy;

namespace U2A2.Models.ViewModels
{
	public class RazaPaisViewModel
	{
		public string Nombre { get; set; } = null!;
		public IEnumerable<RazaModel> Razas { get; set; } = null!;
	}

}
