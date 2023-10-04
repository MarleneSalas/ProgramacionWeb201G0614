namespace U2A1.Models.ViewModels
{
	public class EspecieViewModel
	{
        public int IdClase { get; set; }
		public string NombreClase { get; set; } = null!;
		public IEnumerable<EspecieModel> Especies { get; set; } = null!;
    }

	public class EspecieModel
	{
		public int IdEspecie { get; set; }
		public string NombreEspecie { get; set; } = null!;
	}
}
