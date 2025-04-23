using MediatR;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Application.Features.tipoVenta.Queries.GetByIDPropiedades
{
    public class GetByIDTiposVentaQuery : IRequest<TiposVentaModel>
    {
        public int TipoVentaID { get; set; }
    }

    public class GetByIDTiposVentaQueryHandler : IRequestHandler<GetByIDTiposVentaQuery, TiposVentaModel>
    {
        private readonly ITiposVentaRepository _tiposVentaRepository;

        public GetByIDTiposVentaQueryHandler(ITiposVentaRepository tiposVentaRepository)
        {
            _tiposVentaRepository = tiposVentaRepository;
        }

        public async Task<TiposVentaModel> Handle(GetByIDTiposVentaQuery request, CancellationToken cancellationToken)
        {
            var result = await _tiposVentaRepository.GetById(request.TipoVentaID);

            if (!result.Success || result.Data == null)
                throw new ApplicationException(result.Message ?? "Tipo de venta no encontrado.");

            return (TiposVentaModel)result.Data;
        }
    }
}
