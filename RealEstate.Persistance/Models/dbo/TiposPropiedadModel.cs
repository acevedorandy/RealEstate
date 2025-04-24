

using System.Text.Json.Serialization;

namespace RealEstate.Persistance.Models.dbo
{
    public class TiposPropiedadModel
    {
        public int TipoPropiedadID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        [JsonIgnore]
        public int PropiedadesAsociadas { get; set; }
    }
}
