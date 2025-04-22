

using RealEstate.Persistance.Models.dbo;
using RealEstate.Persistance.Models.ViewModel;

namespace RealEstate.Persistance.Models.EnumerablesModel
{
    public class PropiedadDetallesModel
    {
        public PropiedadesModel PropiedadesModel { get; set; }
        public UsuariosModel UsuariosModel { get; set; }
        public List<PropiedadFotosModel> PropiedadFotosModel { get; set; }
        public List<PropiedadMejorasModelViewModel> MejorasModels { get; set; }


    }
}
