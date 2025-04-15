

using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Repositories;
using RealEstate.Domain.Result;

namespace RealEstate.Persistance.Interfaces.dbo
{
    public interface IMensajesRepository : IBaseRepository<Mensajes>
    {
        Task<OperationResult> GetDestinatario(string remitenteId);
    }
}
