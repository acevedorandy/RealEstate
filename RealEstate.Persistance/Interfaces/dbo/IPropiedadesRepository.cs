using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Repositories;
using RealEstate.Domain.Result;

namespace RealEstate.Persistance.Interfaces.dbo
{
    public interface IPropiedadesRepository : IBaseRepository<Propiedades>
    {
        Task<OperationResult> GetAgentByProperty(int propiedadId);
        Task<OperationResult> GetAllPropertyByAgent(string agenteId);
        Task<OperationResult> MarkAsSold(int propiedadId);
        Task<OperationResult> RemovePropertyWithAll(int propiedadId);
    }
}
