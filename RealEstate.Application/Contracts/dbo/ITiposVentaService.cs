

using RealEstate.Application.Base;
using RealEstate.Application.Core;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Domain.Result;

namespace RealEstate.Application.Contracts.dbo
{
    public interface ITiposVentaService : IBaseService<ServiceResponse, TiposVentaDto>
    {
        Task<ServiceResponse> RemoveTypeSalesWithPropertyAsync(int tipoId);

    }
}
