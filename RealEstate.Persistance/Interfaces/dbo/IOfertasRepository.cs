

using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Repositories;
using RealEstate.Domain.Result;
using RealEstate.Persistance.Context;

namespace RealEstate.Persistance.Interfaces.dbo
{
    public interface IOfertasRepository : IBaseRepository<Ofertas>
    {
        Task<OperationResult> GetPropertyOffered(string clienteId);
        Task<bool> PendingBids(string clienteId);
    }
}

