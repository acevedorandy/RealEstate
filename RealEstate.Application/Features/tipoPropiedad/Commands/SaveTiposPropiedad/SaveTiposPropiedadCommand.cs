using AutoMapper;
using MediatR;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;

namespace RealEstate.Application.Features.tipoPropiedad.Commands.SaveTiposPropiedad
{
    public class SaveTiposPropiedadCommand : IRequest<int>
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }

    public class SaveTiposPropiedadCommandHandler : IRequestHandler<SaveTiposPropiedadCommand, int>
    {
        private readonly ITiposPropiedadRepository _tiposPropiedadRepository;
        private readonly IMapper _mapper;

        public SaveTiposPropiedadCommandHandler(ITiposPropiedadRepository tiposPropiedadRepository, IMapper mapper)
        {
            _tiposPropiedadRepository = tiposPropiedadRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(SaveTiposPropiedadCommand request, CancellationToken cancellationToken)
        {
            var tipoVenta = _mapper.Map<TiposPropiedad>(request);
            var result = await _tiposPropiedadRepository.Save(tipoVenta);

            if (!result.Success || result.Data == null)
                throw new ApplicationException(result.Message ?? "Error al guardar el tipo de venta.");

            return ((TiposPropiedad)result.Data).TipoPropiedadID;
        }
    }
}
