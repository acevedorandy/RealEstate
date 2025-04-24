using RealEstate.Application.Base;
using RealEstate.Application.Core;
using RealEstate.Application.Dtos.dbo;

namespace RealEstate.Application.Contracts.dbo
{
    public interface IMejorasService : IBaseService<ServiceResponse, MejorasDto>
    {
        Task<ServiceResponse> GetMejorasByPropertyAsync(int propiedadId);
        Task<bool> ExisteMejoraAsync(int mejoraId, int propiedadId);

    }
}
