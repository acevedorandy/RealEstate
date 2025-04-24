using MediatR;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;
using RealEstate.Persistance.Models.ViewModel;

namespace RealEstate.Application.Features.agente.Queries.GetByIDAgente
{
    public class GetByIDAgenteQuery : IRequest<UsuariosModel>
    {
        public string Id { get; set; }
    }

    public class GetByIDAgenteQueryHandler : IRequestHandler<GetByIDAgenteQuery, UsuariosModel>
    {
        private readonly IUsuariosRepository _usuariosRepository;

        public GetByIDAgenteQueryHandler(IUsuariosRepository usuariosRepository)
        {
            _usuariosRepository = usuariosRepository;
        }

        public async Task<UsuariosModel> Handle(GetByIDAgenteQuery request, CancellationToken cancellationToken)
        {
            var result = await _usuariosRepository.GetIdentityUserBy(request.Id);

            if (!result.Success || result.Data == null)
                throw new ApplicationException(result.Message ?? "El agente no fue encontrado.");

            return (UsuariosModel)result.Data;
        }
    }
}
