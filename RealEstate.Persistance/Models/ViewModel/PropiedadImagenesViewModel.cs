

namespace RealEstate.Persistance.Models.ViewModel
{
    internal class PropiedadImagenesViewModel
    {
        public List<int> PropiedadID { get; set; } = new List<int>();
        public List<int> RelacionID { get; set; } = new List<int>();
        public List<string> Imagenes { get; set; } = new List<string>();
    }
}
