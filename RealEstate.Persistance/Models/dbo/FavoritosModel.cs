

namespace RealEstate.Persistance.Models.dbo
{
    public class FavoritosModel
    {
        public int FavoritoID { get; set; }
        public string UsuarioID { get; set; }
        public int PropiedadID { get; set; }
        public bool IsFavorito { get; set; }

    }
}
