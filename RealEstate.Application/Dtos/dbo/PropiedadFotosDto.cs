

using Microsoft.AspNetCore.Http;

namespace RealEstate.Application.Dtos.dbo
{
    public class PropiedadFotosDto
    {
        public int RelacionID { get; set; }
        public int PropiedadID { get; set; }
        public string? Imagen { get; set; }
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();

    }
}
