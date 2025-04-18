using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Domain.Entities.dbo
{
    [Table("Reservas", Schema = "dbo")]
    public class Reservas
    {
        [Key]
        public int ReservaID { get; set; }
        public int PropiedadID { get; set; }
        public string ClienteID { get; set; }
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; }

    }
}
