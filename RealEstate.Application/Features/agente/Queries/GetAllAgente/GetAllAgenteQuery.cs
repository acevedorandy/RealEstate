using MediatR;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;
using RealEstate.Persistance.Models.ViewModel;

namespace RealEstate.Application.Features.agente.Queries.GetAllAgente
{
    public class GetAllAgenteQuery : IRequest<IEnumerable<AgentesModel>>
    {
    }

    public class GetAllAgenteQueryHandler : IRequestHandler<GetAllAgenteQuery, IEnumerable<AgentesModel>>
    {
        private readonly IUsuariosRepository _usuariosRepository;

        public GetAllAgenteQueryHandler(IUsuariosRepository usuariosRepository)
        {
            _usuariosRepository = usuariosRepository;
        }

        public async Task<IEnumerable<AgentesModel>> Handle(GetAllAgenteQuery request, CancellationToken cancellationToken)
        {
            var result = await _usuariosRepository.GetAllAgent();

            if(!result.Success || result.Data == null)
                throw new ApplicationException(result.Message ?? "Error al obtener los agentes.");

            return (IEnumerable<AgentesModel>)result.Data;
        }
    }
}
