using MediatR;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Application.Features.propiedad.Queries.GetAllPropiedades
{
    /// <summary>
    ///  No necesario tener parametros aqui
    /// </summary>
    public class GetAllPropiedadesQuery : IRequest<IEnumerable<PropiedadesModel>>
    {
    }

    public class GetAllPropiedadesQueryHandler : IRequestHandler<GetAllPropiedadesQuery, IEnumerable<PropiedadesModel>>
    {
        private readonly IPropiedadesRepository _propiedadesRepository;

        public GetAllPropiedadesQueryHandler(IPropiedadesRepository propiedadesRepository)
        {
            _propiedadesRepository = propiedadesRepository;
        }

        public async Task<IEnumerable<PropiedadesModel>> Handle(GetAllPropiedadesQuery request, CancellationToken cancellationToken)
        {
            var result = await _propiedadesRepository.GetAll();

            if (!result.Success || result.Data == null)
                throw new ApplicationException(result.Message ?? "Error al obtener las propiedades.");
            return (IEnumerable<PropiedadesModel>)result.Data;
        }
    }
}
