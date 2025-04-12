

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Domain.Entities.dbo
{
    [Table("PropiedadFotos", Schema = "dbo")]
    public class PropiedadFotos
    {
        [Key]
        public int RelacionID { get; set; }

        [ForeignKey("Propiedades")]
        public int PropiedadID { get; set; }
        public string? Imagen { get; set; }

    }
}
