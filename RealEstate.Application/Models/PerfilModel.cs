using Microsoft.AspNetCore.Http;

namespace RealEstate.Application.Models
{
    public class PerfilModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public string? Foto { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public IFormFile FotoFile { get; set; }
    }
}
