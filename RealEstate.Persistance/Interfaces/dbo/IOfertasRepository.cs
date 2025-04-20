using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Repositories;
using RealEstate.Domain.Result;

namespace RealEstate.Persistance.Interfaces.dbo
{
    public interface IOfertasRepository : IBaseRepository<Ofertas>
    {
        Task<OperationResult> GetPropertyOffered(string clienteId);
        Task<OperationResult> GetOfferedByMyProperty(int propiedadId);
        Task<OperationResult> GetAllOffersByClient(int propiedadId, string clienteId);
        Task<OperationResult> GetAllExceptId(int ofertaId);
        Task<bool> PendingBids(string clienteId);
    }
}

