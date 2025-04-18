using RealEstate.Application.Base;
using RealEstate.Application.Core;
using RealEstate.Application.Dtos.dbo;

namespace RealEstate.Application.Contracts.dbo
{
    public interface IPropiedadFotosService : IBaseService<ServiceResponse, PropiedadFotosDto>
    {
        Task<ServiceResponse> AddPhotoAsEntity(int propiedadId, List<string> imagePaths);
        Task<ServiceResponse> GetPhotosByPropertyAsync(int propiedadId);

    }
}
