using MediatR;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Application.Features.tipoPropiedad.Queries.GetByIDTiposPropiedad
{
    public class GetByIDTiposPropiedadQuery : IRequest<TiposPropiedadModel>
    {
        public int TipoPropiedadID { get; set; }
    }

    public class GetByIDTiposPropiedadCommandHandler : IRequestHandler<GetByIDTiposPropiedadQuery, TiposPropiedadModel>
    {
        private readonly ITiposPropiedadRepository _tiposPropiedadRepository;

        public GetByIDTiposPropiedadCommandHandler(ITiposPropiedadRepository tiposPropiedadRepository)
        {
            _tiposPropiedadRepository = tiposPropiedadRepository;
        }

        public async Task<TiposPropiedadModel> Handle(GetByIDTiposPropiedadQuery request, CancellationToken cancellationToken)
        {
            var result = await _tiposPropiedadRepository.GetById(request.TipoPropiedadID);

            if (!result.Success || result.Data == null)
                throw new ApplicationException(result.Message ?? "El tipo de propiedad no fue encontrado.");

            return (TiposPropiedadModel)result.Data;
        }
    }
}
