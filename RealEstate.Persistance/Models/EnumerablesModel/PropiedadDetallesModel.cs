

using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Persistance.Models.EnumerablesModel
{
    public class PropiedadDetallesModel
    {
        public PropiedadesModel PropiedadesModel { get; set; }
        public UsuariosModel UsuariosModel { get; set; }
        public List<PropiedadFotosModel> PropiedadFotosModel { get; set; }


    }
}
