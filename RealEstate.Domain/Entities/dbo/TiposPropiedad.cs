using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RealEstate.Domain.Entities.dbo
{
    [Table("TiposPropiedad", Schema = "dbo")]
    public class TiposPropiedad
    {
        [Key]
        public int TipoPropiedadID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
