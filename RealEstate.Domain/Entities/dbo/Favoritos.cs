

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Domain.Entities.dbo
{
    [Table("Favoritos", Schema = "dbo")]
    public class Favoritos
    {
        [Key]
        public int FavoritoID { get; set; }
        public string UsuarioID { get; set; }
        public int PropiedadID { get; set; }
        public bool IsFavorito { get; set; }
    }
}
