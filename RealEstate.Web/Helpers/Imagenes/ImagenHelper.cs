using RealEstate.Application.Dtos.identity.account;
using RealEstate.Web.Helpers.Imagenes.Base;

namespace RealEstate.Web.Helpers.Imagenes
{
    public class ImagenHelper : ILoadPhoto<RegisterDto, IFormFile>
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly LoadPhoto _loadPhoto;

        public ImagenHelper(IWebHostEnvironment webHostEnvironment, LoadPhoto loadPhoto)
        {
            _webHost = webHostEnvironment;
            _loadPhoto = loadPhoto;
        }
        public async Task<RegisterDto> LoadPhoto(RegisterDto dto, IFormFile Foto)
        {
            if (Foto != null && Foto.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "images/propiedades");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string fileExtension = Path.GetExtension(Foto.FileName);
                string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Foto.CopyToAsync(fileStream);
                }

                dto.Foto = "/images/usuario/" + uniqueFileName;
            }
            return dto;
        }
    }
}
