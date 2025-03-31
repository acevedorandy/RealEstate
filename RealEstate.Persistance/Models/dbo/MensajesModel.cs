
namespace RealEstate.Persistance.Models.dbo
{
    public class MensajesModel
    {
        public int MensajeID { get; set; }
        public string RemitenteID { get; set; }
        public string DestinatarioID { get; set; }
        public int PropiedadID { get; set; }
        public string Mensaje { get; set; }
        public DateTime Enviado { get; set; }
        public bool Visto { get; set; }

    }
}
