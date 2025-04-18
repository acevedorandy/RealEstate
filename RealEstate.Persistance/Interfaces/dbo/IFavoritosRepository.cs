
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Identity.Client;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Repositories;
using RealEstate.Domain.Result;

namespace RealEstate.Persistance.Interfaces.dbo
{
    public interface IFavoritosRepository : IBaseRepository<Favoritos>
    {
        Task<OperationResult> GetAllFavoritePropertyByUser(string userId);
        Task<bool> ExistsRelation(int propiedadId, string userId);

    }
}
