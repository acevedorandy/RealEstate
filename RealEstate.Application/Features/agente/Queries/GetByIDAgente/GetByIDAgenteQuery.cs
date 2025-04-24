

using MediatR;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;
using RealEstate.Persistance.Models.ViewModel;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstate.Application.Features.agente.Queries.GetByIDAgente
{
    /// <summary>
    ///  Parametros para obtener un Agente por su Id
    /// </summary>
    public class GetByIDAgenteQuery : IRequest<UsuariosModel>
    {
        /// <example>
        ///  7d3b5ae2-70be-42e7-90a7-8e34770913b3
        /// </example>
        [SwaggerParameter(Description = "Id del Agente del que desea obtener su informacion")]
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
