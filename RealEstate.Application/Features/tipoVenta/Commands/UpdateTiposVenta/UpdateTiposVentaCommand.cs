using System.Text.Json.Serialization;
using AutoMapper;
using MediatR;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstate.Application.Features.tipoVenta.Commands.UpdateTiposVenta
{
    /// <summary>
    ///  Parametros para la actualizacion de un tipo de venta
    /// </summary>
    public class UpdateTiposVentaCommand : IRequest<TiposVentaModel>
    {
        /// <example>
        ///  11
        /// </example>
        [JsonIgnore]
        [SwaggerParameter(Description = "Id del tipo de venta que se desea actualizar")]
        public int TipoVentaID { get; set; }
        [SwaggerParameter(Description = "Nuevo Nombre del tipo de venta")]
        public string Nombre { get; set; }
        [SwaggerParameter(Description = "Una nueva Descripcion para este tipo de venta")]
        public string Descripcion { get; set; }
    }

    public class UpdateTiposVentaCommandHandler : IRequestHandler<UpdateTiposVentaCommand, TiposVentaModel>
    {
        private readonly ITiposVentaRepository _tiposVentaRepository;
        private readonly IMapper _mapper;

        public UpdateTiposVentaCommandHandler(ITiposVentaRepository tiposVentaRepository, IMapper mapper)
        {
            _tiposVentaRepository = tiposVentaRepository;
            _mapper = mapper;
        }

        public async Task<TiposVentaModel> Handle(UpdateTiposVentaCommand request, CancellationToken cancellationToken)
        {
            if(request.TipoVentaID <= 0)
                throw new ArgumentException("ID del tipo de venta no válido.");

            var tipoGetBy = await _tiposVentaRepository.GetById(request.TipoVentaID);

            if(!tipoGetBy.Success || tipoGetBy.Data == null)
                throw new ArgumentException("ID del tipo de venta no existe.");

            var tipo = _mapper.Map<TiposVenta>(tipoGetBy.Data);
            _mapper.Map(request, tipo);

            var result = await _tiposVentaRepository.Update(tipo);

            if (!result.Success)
                throw new ApplicationException(result.Message ?? "Error al actualizar el tipo de venta.");

            return result.Data;
        }
    }
}
