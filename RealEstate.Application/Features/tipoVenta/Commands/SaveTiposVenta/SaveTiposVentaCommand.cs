using AutoMapper;
using MediatR;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstate.Application.Features.tipoVenta.Commands.SaveTiposVenta
{
    /// <summary>
    ///  Parametros para la creacion de un tipo de Venta
    /// </summary>
    public class SaveTiposVentaCommand : IRequest<int>
    {
        /// <example>
        ///  Venta
        /// </example>
        [SwaggerParameter(Description = "Nombre del tipo de venta")]
        public string Nombre { get; set; }
        /// <example>
        ///  Eso es suyo
        /// </example>
        [SwaggerParameter(Description = "Una Descripcion del tipo de venta")]
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
