using MediatR;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Application.Features.agente.Queries.GetAllAgente
{
    public class GetAllAgenteQuery : IRequest<IEnumerable<UsuariosModel>>
    {
    }

    public class GetAllAgenteQueryHandler : IRequestHandler<GetAllAgenteQuery, IEnumerable<UsuariosModel>>
    {
        private readonly IUsuariosRepository _usuariosRepository;

        public GetAllAgenteQueryHandler(IUsuariosRepository usuariosRepository)
        {
            _usuariosRepository = usuariosRepository;
        }

        public async Task<IEnumerable<UsuariosModel>> Handle(GetAllAgenteQuery request, CancellationToken cancellationToken)
        {
            var result = await _usuariosRepository.GetAllAgent();

            if(!result.Success || result.Data == null)
                throw new ApplicationException(result.Message ?? "Error al obtener los agentes.");

            return (IEnumerable<UsuariosModel>)result.Data;
        }
    }
}
