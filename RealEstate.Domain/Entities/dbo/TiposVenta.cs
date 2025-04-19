using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RealEstate.Domain.Entities.dbo
{
    [Table("TiposVenta", Schema = "dbo")]
    public class TiposVenta
    {
        [Key]
        public int TipoVentaID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
