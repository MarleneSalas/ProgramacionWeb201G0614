namespace U2A2.Models.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<char> Letras { get; set; } = null!;
        public IEnumerable<RazaModel> RazasPerro { get; set; }=null!;
    }

    public class RazaModel
    {
        public int IdRaza { get; set; }
        public string Nombre{ get; set; } = null!;
    }

}
