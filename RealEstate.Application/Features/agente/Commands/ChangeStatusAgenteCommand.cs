using System.Text.Json.Serialization;
using MediatR;
using RealEstate.Persistance.Interfaces.dbo;

namespace RealEstate.Application.Features.agente.Commands
{
    public class ChangeStatusAgenteCommand : IRequest<string>
    {
        [JsonIgnore]
        public string? Id { get; set; }
    }

    public class ChangeStatusAgenteCommandHandler : IRequestHandler<ChangeStatusAgenteCommand, string>
    {
        private readonly IUsuariosRepository _usuariosRepository;

        public ChangeStatusAgenteCommandHandler(IUsuariosRepository usuariosRepository)
        {
            _usuariosRepository = usuariosRepository;
        }

        public async Task<string> Handle(ChangeStatusAgenteCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Id))
                throw new ArgumentException("El ID del agente no puede estar vacío.");

            var agenteGet = await _usuariosRepository.GetIdentityUserBy(request.Id);

            if (!agenteGet.Success || agenteGet.Data == null)
                throw new InvalidOperationException("No se encontró el agente con el ID proporcionado.");

            var result = await _usuariosRepository.ActivarOrDesactivar(request.Id);

            if (!result.Success)
                throw new ApplicationException(result.Message ?? "No se pudo cambiar el estado del agente.");

            return request.Id;
        }
    }
}
