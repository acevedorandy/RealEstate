

namespace RealEstate.Web.Helpers.Imagenes.Base
{
    public interface ILoadPhotos<TDto>
    {
        Task<TDto> LoadPhotos(TDto dto, List<IFormFile> fotos, string subFolder);
    }
}