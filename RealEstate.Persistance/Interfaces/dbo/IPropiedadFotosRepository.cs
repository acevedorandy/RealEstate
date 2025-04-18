

using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Repositories;
using RealEstate.Domain.Result;

namespace RealEstate.Persistance.Interfaces.dbo
{
    public interface IPropiedadFotosRepository : IBaseRepository<PropiedadFotos>
    {
        Task<OperationResult> GetPhotosByProperty(int propiedadId);

    }
}
