using MediatR;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;


namespace RealEstate.Application.Features.tipoVenta.Commands.RemoveTiposVenta
{
    public class RemoveTiposVentaCommand : IRequest<int>
    {
        public int TipoVentaID { get; set; }
    }

    public class RemoveTiposVentaCommandHandler : IRequestHandler<RemoveTiposVentaCommand, int>
    {
        private readonly ITiposVentaRepository _tiposVentaRepository;

        public RemoveTiposVentaCommandHandler(ITiposVentaRepository tiposVentaRepository)
        {
            _tiposVentaRepository = tiposVentaRepository;
        }

        public async Task<int> Handle(RemoveTiposVentaCommand request, CancellationToken cancellationToken)
        {
            if(request.TipoVentaID <= 0)
                throw new ArgumentException("ID inválido para el tipo de venta.");

            var tipoGetBy = await _tiposVentaRepository.GetById(request.TipoVentaID);
            if (!tipoGetBy.Success)
                throw new InvalidOperationException("El tipo de venta no existe.");

            var tipoVenta = (TiposVenta)tipoGetBy.Data;
            await _tiposVentaRepository.Remove(tipoVenta);

            return tipoVenta.TipoVentaID;
        }
    }

}
