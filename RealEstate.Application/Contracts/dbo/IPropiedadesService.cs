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
        Task<ServiceResponse > LoadPropertyAsync(int propiedadId);
        Task<ServiceResponse> GetAllFilter(int? tipoPropiedad,string? codigo, decimal? minPrice, decimal? maxPrice, int? habitacion, int? baños);
    }
}
