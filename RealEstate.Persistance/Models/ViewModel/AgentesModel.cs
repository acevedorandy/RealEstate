

namespace RealEstate.Persistance.Models.ViewModel
{
    public class AgentesModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int PropiedadID { get; set; }
        public string Correo { get; set; }
        public bool IsActive { get; set; }
    }
}
