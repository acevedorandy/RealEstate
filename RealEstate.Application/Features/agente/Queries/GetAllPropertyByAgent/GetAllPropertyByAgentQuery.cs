using MediatR;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstate.Application.Features.agente.Queries.GetAllPropertyByAgent
{
    /// <summary>
    ///  Parametros para obtener todas las propiedades asociadas a un Agente
    /// </summary>
    public class GetAllPropertyByAgentQuery : IRequest<IEnumerable<PropiedadesModel>>
    {
        /// <example>
        ///  7d3b5ae2-70be-42e7-90a7-8e34770913b3
        /// </example>
        [SwaggerParameter(Description = "Id del Agente del que desea obtener las propiedades")]
        public string AgenteID { get; set; }
    }

    public class GetAllPropertyByAgentQueryHandler : IRequestHandler<GetAllPropertyByAgentQuery, IEnumerable<PropiedadesModel>>
    {
        private readonly IPropiedadesRepository _propiedadesRepository;

        public GetAllPropertyByAgentQueryHandler(IPropiedadesRepository propiedadesRepository)
        {
            _propiedadesRepository = propiedadesRepository;
        }

        public async Task<IEnumerable<PropiedadesModel>> Handle(GetAllPropertyByAgentQuery request, CancellationToken cancellationToken)
        {
            var result = await _propiedadesRepository.GetAllPropertyByAgent(request.AgenteID);

            if(!result.Success || result.Data == null)
                throw new ApplicationException(result.Message ?? "Error al obtener las propiedades del agente.");

            return (IEnumerable<PropiedadesModel>)result.Data;
        }
    }
}
