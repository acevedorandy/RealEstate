using RealEstate.Application.Base;
using RealEstate.Application.Core;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Domain.Result;


namespace RealEstate.Application.Contracts.dbo
{
    public interface IOfertasService : IBaseService<ServiceResponse, OfertasDto>
    {
        Task<ServiceResponse> GetPropertyOfferedAsync();
        Task<ServiceResponse> GetOfferedByMyPropertyAsync(int propiedadId);
        Task<ServiceResponse> GetAllOffersByClientAsync(int propiedadId, string clienteId);
        Task<bool> PendingBidsAsync();

    }
}
