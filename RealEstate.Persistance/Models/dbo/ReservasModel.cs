

namespace RealEstate.Persistance.Models.dbo
{
    public class ReservasModel
    {
        public int ReservaID { get; set; }
        public int PropiedadID { get; set; }
        public string ClienteID { get; set; }
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; }

    }
}
