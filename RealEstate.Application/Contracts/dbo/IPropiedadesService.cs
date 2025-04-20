using RealEstate.Application.Base;
using RealEstate.Application.Core;
using RealEstate.Application.Dtos.dbo;

namespace RealEstate.Application.Contracts.dbo
{
    public interface IPropiedadesService : IBaseService<ServiceResponse, PropiedadesDto>
    {
        Task<ServiceResponse> GetAgentByPropertyAsync(int propiedadId);
        Task<ServiceResponse> GetAllPropertyByAgentAsync(string id);
        Task<ServiceResponse> GetAllPropertyByAgentLogged();
        Task<ServiceResponse> GetAllPropertyByAgentIncludeSold();
        Task<ServiceResponse> GetAllPropertyNotSold();
        
        Task<ServiceResponse> GetAllFilter(string tipoPropiedad, decimal? minPrice, decimal? maxPrice, int? habitacion, int? baños);
    }
}
