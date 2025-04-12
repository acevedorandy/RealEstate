

using RealEstate.Application.Base;
using RealEstate.Application.Core;
using RealEstate.Application.Dtos.dbo;

namespace RealEstate.Application.Contracts.dbo
{
    public interface IPropiedadesService : IBaseService<ServiceResponse, PropiedadesDto>
    {
        Task<ServiceResponse> GetAgentByPropertyAsync(int propiedadId);
        Task<ServiceResponse> FilterByTypeAsync(string? tipoPropiedad);
        Task<ServiceResponse> FilterByPriceAsync(decimal? minPrice, decimal? maxPrice);
        Task<ServiceResponse> FilterRoomAsync(int? habitacion, int? baños);

    }
}
