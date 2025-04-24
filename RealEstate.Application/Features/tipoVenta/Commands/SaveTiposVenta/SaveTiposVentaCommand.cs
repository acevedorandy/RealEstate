using AutoMapper;
using MediatR;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;

namespace RealEstate.Application.Features.tipoVenta.Commands.SaveTiposVenta
{
    public class SaveTiposVentaCommand : IRequest<int>
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }

    public class SaveTiposVentaCommandHandler : IRequestHandler<SaveTiposVentaCommand, int>
    {
        private readonly ITiposVentaRepository _tiposVentaRepository;
        private readonly IMapper _mapper;

        public SaveTiposVentaCommandHandler(ITiposVentaRepository tiposVentaRepository, IMapper mapper)
        {
            _tiposVentaRepository = tiposVentaRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(SaveTiposVentaCommand request, CancellationToken cancellationToken)
        {
            var tipo = _mapper.Map<TiposVenta>(request);
            var result = await _tiposVentaRepository.Save(tipo);

            if (!result.Success)
                throw new ApplicationException(result.Message ?? "Error al guardar el tipo de venta.");

            return tipo.TipoVentaID;
        }
    }
}
