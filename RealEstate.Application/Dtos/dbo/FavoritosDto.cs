

namespace RealEstate.Application.Dtos.dbo
{
    public class FavoritosDto
    {
        public int FavoritoID { get; set; }
        public string UsuarioID { get; set; }
        public int PropiedadID { get; set; }
        public bool IsFavorito { get; set; }

    }
}
