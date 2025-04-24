using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using RealEstate.Application.Contracts.identity;
using RealEstate.Application.Dtos.identity;
using RealEstate.Application.Responses.identity;
using RealEstate.Identity.Helpers;
using RealEstate.Identity.Shared.Entities;
using RealEstate.Infraestructure.Interfaces;
using RealEstate.Infraestructure.Settings;
using System.IdentityModel.Tokens.Jwt;

namespace RealEstate.Identity.Services
{
    public class AccountServiceForWebApi : BaseAccountService, IAccountServiceForWebApi
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWTHelper _jwtHelper;

        public AccountServiceForWebApi(UserManager<ApplicationUser> userManager,
                              IEmailService emailService,
                              EmailHelper emailHelper,
                              JWTHelper jWTHelper,
                              IMapper mapper,
                              IOptions<JWTSettings> jwtSettings
                              ) : base(userManager, emailService, emailHelper, jwtSettings, mapper)
        {
            _userManager = userManager;
            _jwtHelper = jWTHelper;

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

            var isPasswordValid = await _userManager.CheckPasswordAsync(usuario, authenticationRequest.Password);
            if (!isPasswordValid)
                return SetError($"Credenciales inválidas para {authenticationRequest.Email}");

            if (!usuario.EmailConfirmed)
                return SetError($"Se necesita la activación del correo: {authenticationRequest.Email} para iniciar sesión.");

            JwtSecurityToken jwtSecurityToken = await _jwtHelper.GenerateJWToken(usuario);

            response.Id = usuario.Id;
            response.Email = usuario.Email;
            response.UserName = usuario.UserName;
            response.Roles = (await _userManager.GetRolesAsync(usuario)).ToList();
            response.IsVerified = usuario.EmailConfirmed;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var refreshToken = _jwtHelper.GenerateRefreshToken();
            response.RefreshToken = refreshToken.Token;

            return response;
        }

    }
}
