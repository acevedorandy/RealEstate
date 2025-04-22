using RealEstate.Application.Core;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Application.Dtos.identity.account;
using RealEstate.Application.Responses.identity;
using RealEstate.Domain.Result;


namespace RealEstate.Application.Contracts.dbo
{
    public interface IUsuariosService
    {
        /* Metodos De Las Cuentas */
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto, string origin);
        Task<AuthenticationResponse> LoginAsync(LoginDto loginDto);
        Task<RegisterResponse> RegisterAsync(RegisterDto registerDto, string origin);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task SignOutAsync();


        /* Metodos Generales Con Las Cuentas */
        Task<ServiceResponse> GetIdentityUserAllAsync();
        Task<ServiceResponse> GetIdentityUserByAsync(string userId);
        Task<ServiceResponse> GetUserByRolAsync(string rol);
        Task<ServiceResponse> ActivarOrDesactivarAsync(string userId);
        Task<ServiceResponse> GetAgentActiveAsync();
        Task<ServiceResponse> GetAgentByNameAsync(string name);
        Task<ServiceResponse> LoadHomeView();
        Task<ServiceResponse> GetAllAgentAsync();
        Task<ServiceResponse> GetAllDeveloperAsync();
        Task<ServiceResponse> GetAllAdminsAsync();
        Task<ServiceResponse> RemoveAgentWithPropertyAsync(string userId);
        Task<ServiceResponse> UpdateIdentityUserAsync(UsuariosDto user);
        Task<ServiceResponse> GetPerfilInformation(string id);

    }
}
