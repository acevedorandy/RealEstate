
using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Repositories;
using RealEstate.Domain.Result;

namespace RealEstate.Persistance.Interfaces.dbo
{
    public interface ITiposPropiedadRepository : IBaseRepository<TiposPropiedad>
    {
        Task<OperationResult> RemoveTypeWithProperty(int tipoId);
    }
}
