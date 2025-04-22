

namespace RealEstate.Persistance.Models.dbo
{
    public class TiposPropiedadModel
    {
        public int TipoPropiedadID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int PropiedadesAsociadas { get; set; }
    }
}
