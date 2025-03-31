
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Domain.Entities.dbo
{
    [Table("Contratos", Schema = "dbo")]
    public class Contratos
    {
        [Key]
        public int ContratoID { get; set; }
        public int PropiedadID { get; set; }
        public string ClienteID { get; set; }
        public string AgenteID { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string TipoContrato { get; set; }
        public decimal Monto { get; set; }

    }
}
