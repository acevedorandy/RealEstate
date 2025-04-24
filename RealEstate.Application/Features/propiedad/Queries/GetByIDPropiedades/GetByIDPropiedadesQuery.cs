using MediatR;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstate.Application.Features.propiedad.Queries.GetByIDPropiedades
{
    /// <summary>
    ///  Parametros para obtener una propiedad por su Id
    /// </summary>
    public class GetByIDPropiedadesQuery : IRequest<PropiedadesModel>
    {
        /// <example>
        ///  9
        /// </example>
        [SwaggerParameter(Description = "Id de la propiedad de la que desea obtener su informacion")]
        public int PropiedadID { get; set; }
    }

    public class GetByIDPropiedadesHandler : IRequestHandler<GetByIDPropiedadesQuery, PropiedadesModel>
    {
        private readonly IPropiedadesRepository _propiedadesRepository;

        public GetByIDPropiedadesHandler(IPropiedadesRepository propiedadesRepository)
        {
            _propiedadesRepository = propiedadesRepository;
        }

        public async Task<PropiedadesModel> Handle(GetByIDPropiedadesQuery request, CancellationToken cancellationToken)
        {
            var result = await _propiedadesRepository.GetById(request.PropiedadID);

            if (!result.Success || result.Data == null)
                throw new ApplicationException(result.Message ?? "La propiedad no fue encontrada.");

            return (PropiedadesModel)result.Data;
        }
    }
}
