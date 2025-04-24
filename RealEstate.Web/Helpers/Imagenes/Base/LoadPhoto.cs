using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using RealEstate.Domain.Entities.dbo;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RealEstate.Web.Helpers.Imagenes.Base
{
    public class LoadPhoto
    {
        private readonly IWebHostEnvironment _webHost;

        public LoadPhoto(IWebHostEnvironment webHostEnvironment)
        {
            _webHost = webHostEnvironment;
        }

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            string uploadsFolder = Path.Combine(_webHost.WebRootPath, "images", "propiedades");

            Directory.CreateDirectory(uploadsFolder);

            string fileExtension = Path.GetExtension(file.FileName);
            string uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/images/{"propiedades"}/{uniqueFileName}";
        }
    }
}