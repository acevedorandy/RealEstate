using RealEstate.Application.Dtos.dbo;
using RealEstate.Web.Helpers.Imagenes.Base;


namespace RealEstate.Web.Helpers.Imagenes
{
    public class ImagenHelper
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly LoadPhoto _loadPhoto;

        public ImagenHelper(IWebHostEnvironment webHostEnvironment, LoadPhoto loadPhoto)
        {
            _webHost = webHostEnvironment;
            _loadPhoto = loadPhoto;
        }

        public async Task<List<string>> SavePropertyPhotos(PropiedadesDto dto)
        {
            var imagePaths = new List<string>();

            if (dto.Files == null || !dto.Files.Any())
                return imagePaths;

            foreach (var foto in dto.Files)
            {
                if (foto.Length > 0 && IsValidImage(foto))
                {
                    var savedPath = await _loadPhoto.SaveFileAsync(foto);
                    if (!string.IsNullOrEmpty(savedPath))
                    {
                        imagePaths.Add(savedPath);
                    }
                }
            }
            return imagePaths;
        }

        public async Task<List<string>> UpdatePropertyPhoto(PropiedadesDto dto)
        {
            var imagePaths = new List<string>();

            if (dto.Files == null || !dto.Files.Any())
                return imagePaths;

            foreach (var foto in dto.Files)
            {
                if (foto.Length > 0 && IsValidImage(foto))
                {
                    var savedPath = await _loadPhoto.SaveFileAsync(foto);
                    if (!string.IsNullOrEmpty(savedPath))
                    {
                        imagePaths.Add(savedPath);
                    }
                }
            }
            return imagePaths;
        }

        public async Task<UsuariosDto> UpdatePerfilPhoto(UsuariosDto dto, IFormFile Foto)
        {
            if (Foto != null && Foto.Length > 0)
            {
                if (!string.IsNullOrEmpty(dto.Foto))
                {
                    string oldPhotoPath = dto.Foto;
                    string fullOldPhotoPath = Path.Combine(_webHost.WebRootPath, oldPhotoPath.TrimStart('/'));
                    if (File.Exists(fullOldPhotoPath))
                    {
                        File.Delete(fullOldPhotoPath);
                    }
                }

                string filePath = await _loadPhoto.SaveFileAsync(Foto);

                if (!string.IsNullOrEmpty(filePath))
                {
                    dto.Foto = filePath;
                }
            }
            return dto;
        }

            private bool IsValidImage(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            return allowedExtensions.Contains(extension);
        }
    }
}