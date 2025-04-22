using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Domain.Entities.dbo
{
    [Table("Mejoras", Schema = "dbo")]
    public class Mejoras
    {
        [Key]
        public int MejoraID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
