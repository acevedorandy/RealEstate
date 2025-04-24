using MediatR;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstate.Application.Features.propiedad.Queries.GetByCodePropiedades
{
    /// <summary>
    ///  Parametros para obtener una Propiedad por medo de su Codigo
    /// </summary>
    public class GetByCodePropiedadesQuery : IRequest<PropiedadesModel>
    {
        /// <example>
        ///  48962
        /// </example>
        [SwaggerParameter(Description = "Codigo de la propiedad a obtener")]
        public string Codigo { get; set; }
    }

    public class GetByCodePropiedadesQueryHandler : IRequestHandler<GetByCodePropiedadesQuery, PropiedadesModel>
    {
        private readonly IPropiedadesRepository _propiedadesRepository;

        public GetByCodePropiedadesQueryHandler(IPropiedadesRepository propiedadesRepository)
        {
            _propiedadesRepository = propiedadesRepository;
        }

        public async Task<PropiedadesModel> Handle(GetByCodePropiedadesQuery request, CancellationToken cancellationToken)
        {
            var result = await _propiedadesRepository.GetPropertyByCode(request.Codigo);

            if (!result.Success || result.Data == null)
                throw new ApplicationException(result.Message ?? "La propiedad no fue encontrada.");

            return (PropiedadesModel)result.Data;
        }
    }


}
