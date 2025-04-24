

using RealEstate.Domain.Entities.dbo;
using RealEstate.Domain.Repositories;
using RealEstate.Domain.Result;

namespace RealEstate.Persistance.Interfaces.dbo
{
    public interface IMensajesRepository : IBaseRepository<Mensajes>
    {
        Task<OperationResult> GetDestinatario(string remitenteId);

        Task<OperationResult> GetConversation(int propiedadId, string destinatarioId, string remitenteId);
        Task<OperationResult> GetChatsByClient(string clienteId);
        Task<OperationResult> GetChatsByAgent(string agenteId);

    }
}
