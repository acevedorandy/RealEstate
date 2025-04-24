

using System.Text.Json.Serialization;
using AutoMapper;
using MediatR;
using RealEstate.Application.Enum;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Application.Features.tipoPropiedad.Commands.UpdateTiposPropiedad
{
    public class UpdateTiposPropiedadCommand : IRequest<TiposPropiedadModel>
    {
        [JsonIgnore]
        public int TipoPropiedadID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }

    public class UpdateTiposPropiedadCommandHandler : IRequestHandler<UpdateTiposPropiedadCommand, TiposPropiedadModel>
    {
        private readonly ITiposPropiedadRepository _tiposPropiedadRepository;
        private readonly IMapper _mapper;

        public UpdateTiposPropiedadCommandHandler(ITiposPropiedadRepository tiposPropiedadRepository, IMapper mapper)
        {
            _tiposPropiedadRepository = tiposPropiedadRepository;
            _mapper = mapper;
        }

        public async Task<TiposPropiedadModel> Handle(UpdateTiposPropiedadCommand request, CancellationToken cancellationToken)
        {
            if(request.TipoPropiedadID <= 0)
                throw new ArgumentException("ID del tipo de propiedad no válido.");

            var tipoGetBy = await _tiposPropiedadRepository.GetById(request.TipoPropiedadID);

            if (!tipoGetBy.Success || tipoGetBy.Data == null)
                throw new InvalidOperationException("El tipo de propiedad no existe.");

            var tipoPropiedad = _mapper.Map<TiposPropiedad>(tipoGetBy.Data);
            _mapper.Map(request, tipoPropiedad);

            var result = await _tiposPropiedadRepository.Update(tipoPropiedad);

            return result.Data;
        }
    }
}
