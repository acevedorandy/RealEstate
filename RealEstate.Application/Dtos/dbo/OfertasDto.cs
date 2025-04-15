

namespace RealEstate.Application.Dtos.dbo
{
    public class OfertasDto
    {
        public int OfertaID { get; set; }
        public string ClienteID { get; set; }
        public int PropiedadID { get; set; }
        public decimal Cifra { get; set; }
        public DateTime FechaOferta { get; set; }
        public string Estado { get; set; }
        public bool Aceptada { get; set; }

    }
}
