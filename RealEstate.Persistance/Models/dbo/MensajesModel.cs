
namespace RealEstate.Persistance.Models.dbo
{
    public class MensajesModel
    {
        public int MensajeID { get; set; }
        public string RemitenteID { get; set; }
        public string DestinatarioID { get; set; }
        public int PropiedadID { get; set; }
        public string Codigo { get; set; }
        public string Mensaje { get; set; }
        public DateTime Enviado { get; set; }
        public bool Visto { get; set; }
        public string DestinatarioNombre { get; set; }
        public string RemitenteNombre { get; set; }
        public string PropiedadNombre { get; set; }
        public bool EsRemitente { get; set; } // Cambiado a bool

    }
}
