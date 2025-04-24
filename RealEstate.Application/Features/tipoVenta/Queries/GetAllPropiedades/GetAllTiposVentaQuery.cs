using MediatR;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Application.Features.tipoVenta.Queries.GetAllPropiedades
{
    /// <summary>
    ///  No necesario tener parametros aqui
    /// </summary>
    public class GetAllTiposVentaQuery : IRequest<IEnumerable<TiposVentaModel>>
    {
    }
    public class GetAllTiposVentaQueryHandler : IRequestHandler<GetAllTiposVentaQuery, IEnumerable<TiposVentaModel>>
    {
        private readonly ITiposVentaRepository _tiposVentaRepository;

        public GetAllTiposVentaQueryHandler(ITiposVentaRepository tiposVentaRepository)
        {
            _tiposVentaRepository = tiposVentaRepository;
        }

        public async Task<IEnumerable<TiposVentaModel>> Handle(GetAllTiposVentaQuery request, CancellationToken cancellationToken)
        {
            var result = await _tiposVentaRepository.GetAll();

            if (!result.Success || result.Data == null)
                throw new ApplicationException(result.Message ?? "Error al obtener los tipos de venta.");

            return (IEnumerable<TiposVentaModel>)result.Data;
        }
    }
}
