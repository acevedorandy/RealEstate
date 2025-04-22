

using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Repositories;
using RealEstate.Domain.Result;

namespace RealEstate.Persistance.Interfaces.dbo
{
    public interface ITiposVentaRepository : IBaseRepository<TiposVenta>
    {
        Task<OperationResult> RemoveTypeSalesWithProperty(int tipoId);
    }
}
