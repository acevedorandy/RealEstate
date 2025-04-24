

using RealEstate.Application.Base;
using RealEstate.Application.Core;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Domain.Result;

namespace RealEstate.Application.Contracts.dbo
{
    public interface IMensajesService : IBaseService<ServiceResponse, MensajesDto>
    {
        Task<ServiceResponse> GetConversationAsync(int propiedadId, string destinatarioId);
        Task<ServiceResponse> GetConversationAsAgentAsync(int propiedadId, string destinatarioId, string remitenteId);
        Task<ServiceResponse> GetDestinatarioAsync();
        Task<ServiceResponse> SendFirstMessage(MensajesDto dto);
        Task<ServiceResponse> GetChatsByClientAsync();
        Task<ServiceResponse> GetChatsByAgentAsync();

    }
}
