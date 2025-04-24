using System.Text.Json.Serialization;
using AutoMapper;
using MediatR;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;


namespace RealEstate.Application.Features.tipoVenta.Commands.RemoveTiposVenta
{
    public class RemoveTiposVentaCommand : IRequest<int>
    {
        [JsonIgnore]
        public int TipoVentaID { get; set; }
    }

    public class RemoveTiposVentaCommandHandler : IRequestHandler<RemoveTiposVentaCommand, int>
    {
        private readonly ITiposVentaRepository _tiposVentaRepository;
        private readonly IMapper _mapper;

        public RemoveTiposVentaCommandHandler(ITiposVentaRepository tiposVentaRepository, IMapper mapper)
        {
            _tiposVentaRepository = tiposVentaRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(RemoveTiposVentaCommand request, CancellationToken cancellationToken)
        {
            if(request.TipoVentaID <= 0)
                throw new ArgumentException("ID inválido para el tipo de venta.");

            var tipoGetBy = await _tiposVentaRepository.GetById(request.TipoVentaID);
            if (!tipoGetBy.Success)
                throw new InvalidOperationException("El tipo de venta no existe.");

            var tipoVenta = _mapper.Map<TiposVenta>(tipoGetBy.Data);

            await _tiposVentaRepository.Remove(tipoVenta);

            return request.TipoVentaID;
        }
    }

}
