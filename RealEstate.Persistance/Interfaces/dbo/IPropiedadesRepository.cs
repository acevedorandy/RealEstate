using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Repositories;
using RealEstate.Domain.Result;

namespace RealEstate.Persistance.Interfaces.dbo
{
    public interface IPropiedadesRepository : IBaseRepository<Propiedades>
    {
        Task<OperationResult> GetAgentByProperty(int propiedadId);
        Task<OperationResult> GetAllPropertyByAgent(string agenteId);
        //Task<OperationResult> FilterByType(string? tipoPropiedad);
        //Task<OperationResult> FilterByPrice(decimal? minPrice, decimal? maxPrice);
        //Task<OperationResult> FilterRoom(int? habitacion, int? baños);
    }
}
