

namespace RealEstate.Persistance.Models.ViewModel
{
    public class OfertasViewModel
    {
        public int OfertaID { get; set; }
        public string ClienteID { get; set; }
        public int PropiedadID { get; set; }
        public string Codigo { get; set; }
        public string Titulo { get; set; }
        public string TipoPropiedad { get; set; }
        public string Imagen { get; set; }
        public string NombreAgente { get; set; }
        public string ApellidoAgente { get; set; }
        public decimal Cifra { get; set; }
        public DateTime FechaOferta { get; set; }
        public string Estado { get; set; }
        public bool Aceptada { get; set; }
    }
}
