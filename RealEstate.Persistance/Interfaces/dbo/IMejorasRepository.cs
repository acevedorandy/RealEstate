

using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Repositories;
using RealEstate.Domain.Result;

namespace RealEstate.Persistance.Interfaces.dbo
{
    public interface IMejorasRepository : IBaseRepository<Mejoras>
    {
        Task<OperationResult> GetMejorasByProperty(int propiedadId);
        Task<bool> ExisteMejora(int mejoraId, int propiedadId);

    }
}
