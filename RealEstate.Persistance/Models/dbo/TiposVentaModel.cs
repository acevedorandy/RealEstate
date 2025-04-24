

using System.Text.Json.Serialization;

namespace RealEstate.Persistance.Models.dbo
{
    public class TiposVentaModel
    {
        public int TipoVentaID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        [JsonIgnore]
        public int PropiedadesAsociadas { get; set; }
    }
}
