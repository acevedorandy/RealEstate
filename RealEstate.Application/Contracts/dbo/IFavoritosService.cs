

using RealEstate.Application.Base;
using RealEstate.Application.Core;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Domain.Result;

namespace RealEstate.Application.Contracts.dbo
{
    public interface IFavoritosService : IBaseService<ServiceResponse, FavoritosDto>
    {
        Task<ServiceResponse> GetAllFavoritePropertyByUserAsync();
        Task<bool> ExistsRelationAsync(int propiedadId);

    }
}
