using MediatR;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Application.Features.agente.Queries.GetAllPropertyByAgent
{
    public class GetAllPropertyByAgentQuery : IRequest<IEnumerable<PropiedadesModel>>
    {
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
