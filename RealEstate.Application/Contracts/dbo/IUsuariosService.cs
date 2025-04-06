

using RealEstate.Application.Dtos.identity.account;
using RealEstate.Application.Responses.identity;

namespace RealEstate.Application.Contracts.dbo
{
    public interface IUsuariosService
    {
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto, string origin);
        Task<AuthenticationResponse> LoginAsync(LoginDto loginDto);
        Task<RegisterResponse> RegisterAsync(RegisterDto registerDto, string origin);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task SignOutAsync();
    }
}
