using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Domain.Entities.dbo
{
    [Table("Mensajes", Schema = "dbo")]

    public class Mensajes
    {
        [Key]
        public int MensajeID { get; set; }
        public string RemitenteID { get; set; }
        public string DestinatarioID { get; set; }
        public int PropiedadID { get; set; }
        public string Mensaje { get; set; }
        public DateTime Enviado { get; set; }
        public bool Visto { get; set; }
    }
}
