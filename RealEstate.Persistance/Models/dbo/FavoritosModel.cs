

namespace RealEstate.Persistance.Models.dbo
{
    public class FavoritosModel
    {
        public string UsuarioID { get; set; }
        public int PropiedadID { get; set; }
        public bool IsFavoritos { get; set; }

    }
}
