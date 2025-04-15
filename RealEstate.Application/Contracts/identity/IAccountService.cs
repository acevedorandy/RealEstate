
using RealEstate.Application.Dtos.identity;
using RealEstate.Application.Responses.identity;

namespace RealEstate.Application.Contracts.identity
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest authenticationRequest);
        Task SignOutAsync();
        Task<RegisterResponse> RegisterIdentityAsync(RegisterRequest request, string origin);
        Task<string> ConfirmAccountAsync(string userId, string token);
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request);
    }
}
