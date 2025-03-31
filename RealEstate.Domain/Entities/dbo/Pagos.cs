using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Domain.Entities.dbo
{
    [Table("Pagos", Schema = "dbo")]

    public class Pagos
    {
        [Key]
        public int PagoID { get; set; }
        public int ContratoID { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; }

    }
}
