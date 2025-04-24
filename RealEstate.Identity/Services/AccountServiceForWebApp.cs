using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using RealEstate.Application.Contracts.identity;
using RealEstate.Application.Dtos.identity;
using RealEstate.Application.Responses.identity;
using RealEstate.Identity.Helpers;
using RealEstate.Identity.Shared.Entities;
using RealEstate.Infraestructure.Interfaces;
using RealEstate.Infraestructure.Settings;

namespace RealEstate.Identity.Services
{
    public class AccountServiceForWebApp : BaseAccountService, IAccountServiceForWebApp
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountServiceForWebApp(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              IEmailService emailService,
                              EmailHelper emailHelper,
                              IOptions<JWTSettings> jwtSettings
                              ) : base(userManager, emailService, emailHelper, jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest authenticationRequest)
        {
            AuthenticationResponse response = new AuthenticationResponse();

            AuthenticationResponse SetError(string errorMessage)
            {
                response.HasError = true;
                response.Error = errorMessage;
                return response;
            }

            var usuario = await _userManager.FindByEmailAsync(authenticationRequest.Email);
            if (usuario == null)
                return SetError($"No hay cuentas registradas con el correo {authenticationRequest.Email}");

            var result = await _signInManager.PasswordSignInAsync(usuario.UserName, authenticationRequest.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
                return SetError($"Credenciales inválidas para {authenticationRequest.Email}");

            if (!usuario.EmailConfirmed)
                return SetError($"Se necesita la activación del correo: {authenticationRequest.Email} para iniciar sesión.");

            response.Id = usuario.Id;
            response.Email = usuario.Email;
            response.UserName = usuario.UserName;
            response.Roles = (await _userManager.GetRolesAsync(usuario)).ToList();
            response.IsVerified = usuario.EmailConfirmed;

            return response;
        }
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
