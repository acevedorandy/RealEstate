using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Application.Dtos.identity.account;
using RealEstate.Web.Helpers.Imagenes.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

        //public async Task<PropiedadesDto> UpdatePropertyPhoto(PropiedadesDto dto)
        //{
        //    if (dto.Files != null && dto.Files.Any())
        //    {
        //        if (!string.IsNullOrEmpty(dto.Imagen))
        //        {
        //            string oldPhotoPath = dto.Imagen;
        //            string fullOldPhotoPath = Path.Combine(_webHost.WebRootPath, oldPhotoPath.TrimStart('/'));
        //            if (File.Exists(fullOldPhotoPath))
        //            {
        //                File.Delete(fullOldPhotoPath);
        //            }
        //        }

        //        string filePath = await _loadPhoto.SaveFileAsync(Foto);

        //        if (!string.IsNullOrEmpty(filePath))
        //        {
        //            dto.Imagen = filePath;
        //        }
        //    }
        //    return dto;
        //}

        private bool IsValidImage(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            return allowedExtensions.Contains(extension);
        }
    }
}