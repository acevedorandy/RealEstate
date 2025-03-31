

namespace RealEstate.Persistance.Models.dbo
{
    public class PagosModel
    {
        public int PagoID { get; set; }
        public int ContratoID { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; }

    }
}
