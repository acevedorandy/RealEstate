

namespace RealEstate.Persistance.Models.dbo
{
    public class PropiedadesModel
    {
        public int PropiedadID { get; set; }
        public string Codigo { get; set; }
        public string AgenteID { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public string Sector { get; set; }
        public string CodigoPostal { get; set; }
        public int Habitaciones { get; set; }
        public int Baños { get; set; }
        public int Parqueos { get; set; }
        public decimal TamañoTerreno { get; set; }
        public int TotalNivel { get; set; }
        public int Piso { get; set; }
        public DateTime AñoConstruccion { get; set; }
        public string TipoPropiedad { get; set; }
        public bool Disponibilidad { get; set; }
        public string Imagen { get; set; }
        public bool? Vendida { get; set; }


    }
}
