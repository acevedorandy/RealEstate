

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Domain.Entities.dbo
{
    [Table("PropiedadTiposVenta", Schema = "dbo")]
    public class PropiedadTiposVenta
    {
        [Key]
        public int PropiedadTipoVentaID { get; set; }
        public int PropiedadID { get; set; }
        public int TipoVentaID { get; set; }
    }
}
