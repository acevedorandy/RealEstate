

namespace RealEstate.Application.Dtos.dbo
{
    public class ReservasDto
    {
        public int ReservaID { get; set; }
        public int PropiedadID { get; set; }
        public string ClienteID { get; set; }
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; }

    }
}
