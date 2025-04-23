using MediatR;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;

namespace RealEstate.Application.Features.tipoPropiedad.Commands.RemoveTiposPropiedad
{
    public class RemoveTiposPropiedadCommand : IRequest<int>
    {
        public int TipoPropiedadID { get; set; }
    }

    public class RemoveTiposPropiedadCommandHander : IRequestHandler<RemoveTiposPropiedadCommand, int>
    {
        private readonly ITiposPropiedadRepository _tiposPropiedadRepository;

        public RemoveTiposPropiedadCommandHander(ITiposPropiedadRepository tiposPropiedadRepository)
        {
            _tiposPropiedadRepository = tiposPropiedadRepository;
        }

        public async Task<int> Handle(RemoveTiposPropiedadCommand request, CancellationToken cancellationToken)
        {
            if (request.TipoPropiedadID <= 0)
                throw new ArgumentException("ID inválido para el tipo propiedad.");

            var tipoGetBy = await _tiposPropiedadRepository.GetById(request.TipoPropiedadID);

            if (!tipoGetBy.Success || tipoGetBy.Data == null)
                throw new InvalidOperationException("El tipo de propiedad no existe.");

            var tipoPropiedad = (TiposPropiedad)tipoGetBy.Data;
            await _tiposPropiedadRepository.Remove(tipoPropiedad);

            return request.TipoPropiedadID;
        }
    }
}
