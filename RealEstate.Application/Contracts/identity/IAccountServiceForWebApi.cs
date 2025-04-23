
using RealEstate.Application.Dtos.identity;
using RealEstate.Application.Responses.identity;

namespace RealEstate.Application.Contracts.identity
{
    public interface IAccountServiceForWebApi : IAccountService
    {

        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest authenticationRequest);
    }
}
