

using RealEstate.Domain.Result;

namespace RealEstate.Persistance.Interfaces.dbo
{
    public interface IUsuariosRepository
    {
        Task<OperationResult> GetIdentityUserAll();
        Task<OperationResult> GetIdentityUserBy(string userId);
        Task<OperationResult> GetUserByRol(string rol);
        Task<OperationResult> ActivarOrDesactivar(string userId);
        Task<OperationResult> GetAgentActive();
        Task<OperationResult> GetAgentByName(string name);
    }
}
