

using RealEstate.Domain.Result;
using RealEstate.Identity.Shared.Entities;

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
        Task<OperationResult> GetAllAgent();
        Task<OperationResult> GetAllDeveloper();
        Task<OperationResult> GetAllAdmins();
        Task<OperationResult> RemoveAgentWithProperty(string userId);
        Task<OperationResult> UpdateIdentityUser(ApplicationUser user);
        Task<OperationResult> UpdatePhotoIdentityUser(string Id, string foto);
    }
}
