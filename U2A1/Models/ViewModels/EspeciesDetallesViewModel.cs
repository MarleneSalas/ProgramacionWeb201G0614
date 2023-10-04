namespace U2A1.Models.ViewModels
{
    public class EspeciesDetallesViewModel
    {
        public int Id { get; set; }

        public string Especie { get; set; } = null!;

        public int? IdClase { get; set; }

        public string Habitat { get; set; } = null!;

        public double Peso { get; set; }

        public int Tamaño { get; set; }

        public string Observaciones { get; set; } = null!;
        public string Clase { get; set; } = null!;
    }
}
