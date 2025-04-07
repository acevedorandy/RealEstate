

namespace RealEstate.Application.Dtos.dbo
{
    public class ContratosDto
    {
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
