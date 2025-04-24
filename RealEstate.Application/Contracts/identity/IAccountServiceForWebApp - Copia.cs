
using RealEstate.Application.Dtos.identity;
using RealEstate.Application.Responses.identity;

namespace RealEstate.Application.Contracts.identity
{
    public interface IAccountServiceForWebApp : IAccountService
    {

        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest authenticationRequest);
        Task SignOutAsync();
    }
}
