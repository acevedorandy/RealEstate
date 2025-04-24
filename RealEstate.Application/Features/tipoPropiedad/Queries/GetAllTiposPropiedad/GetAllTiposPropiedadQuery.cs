using MediatR;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Application.Features.tipoPropiedad.Queries.GetAllTiposPropiedad
{
    /// <summary>
    ///  No necesario tener parametros aqui
    /// </summary>
    public class GetAllTiposPropiedadQuery : IRequest<IEnumerable<TiposPropiedadModel>>
    {
    }

    public class GetAllTiposPropiedadCommandHandler : IRequestHandler<GetAllTiposPropiedadQuery, IEnumerable<TiposPropiedadModel>>
    {
        private readonly ITiposPropiedadRepository _tiposPropiedadRepository;

        public GetAllTiposPropiedadCommandHandler(ITiposPropiedadRepository tiposPropiedadRepository)
        {
            _tiposPropiedadRepository = tiposPropiedadRepository;
        }

        public async Task<IEnumerable<TiposPropiedadModel>> Handle(GetAllTiposPropiedadQuery request, CancellationToken cancellationToken)
        {
            var result = await _tiposPropiedadRepository.GetAll();

            if(!result.Success || result.Data == null)
                throw new ApplicationException(result.Message ?? "Error al obtener los tipos de propiedad.");

            return (IEnumerable<TiposPropiedadModel>)result.Data;
        }
    }
}
