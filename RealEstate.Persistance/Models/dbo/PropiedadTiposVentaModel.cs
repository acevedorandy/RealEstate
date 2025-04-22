

namespace RealEstate.Persistance.Models.dbo
{
    public class PropiedadTiposVentaModel
    {
        public int PropiedadTipoVentaID { get; set; }
        public int PropiedadID { get; set; }
        public int TipoVentaID { get; set; }
        public int PropiedadesAsociadas { get; set; }
    }
}
