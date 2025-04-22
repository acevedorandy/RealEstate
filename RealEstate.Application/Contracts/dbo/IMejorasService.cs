

using RealEstate.Application.Base;
using RealEstate.Application.Core;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Domain.Result;

namespace RealEstate.Application.Contracts.dbo
{
    public interface IMejorasService : IBaseService<ServiceResponse, MejorasDto>
    {
        Task<ServiceResponse> GetMejorasByPropertyAsync(int propiedadId);

    }
}
