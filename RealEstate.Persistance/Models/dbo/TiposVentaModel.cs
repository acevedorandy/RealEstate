

namespace RealEstate.Persistance.Models.dbo
{
    public class TiposVentaModel
    {
        public int TipoVentaID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int PropiedadesAsociadas { get; set; }
    }
}
