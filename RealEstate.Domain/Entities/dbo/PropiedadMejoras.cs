
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Domain.Entities.dbo
{
    [Table("PropiedadMejoras", Schema = "dbo")]
    public class PropiedadMejoras
    {
        [Key]
        public int PropiedadMejoraID { get; set; }
        public int PropiedadID { get; set; }
        public int MejoraID { get; set; }
    }
}
