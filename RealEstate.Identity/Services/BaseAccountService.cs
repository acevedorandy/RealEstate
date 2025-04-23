using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using RealEstate.Application.Contracts.identity;
using RealEstate.Application.Dtos.identity;
using RealEstate.Application.Enum;
using RealEstate.Application.Responses.identity;
using RealEstate.Identity.Helpers;
using RealEstate.Identity.Shared.Entities;
using RealEstate.Infraestructure.Interfaces;
using RealEstate.Infraestructure.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace RealEstate.Identity.Services
{
    public abstract class BaseAccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly EmailHelper _emailHelper;

        protected BaseAccountService(UserManager<ApplicationUser> userManager,
                              IEmailService emailService,
                              EmailHelper emailHelper,
                              IOptions<JWTSettings> jwtSettings
                              )
        {
            _userManager = userManager;
            _emailService = emailService;
            _emailHelper = emailHelper;
        }
        public async Task<string> ConfirmAccountAsync(string userId, string token)
        {
            async Task<string> SetError(string message) => message;

            var usuario = await _userManager.FindByIdAsync(userId);
            if (usuario == null)
                return await SetError("Ninguna cuenta registrada con este usuario.");

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(usuario, token);

            return await SetError(result.Succeeded
                ? $"La cuenta con el correo {usuario.Email} ha sido confirmada."
                : $"Ha ocurrido un error registrando el correo {usuario.Email}.");
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            ForgotPasswordResponse response = new ForgotPasswordResponse();

            var usuario = await _userManager.FindByEmailAsync(request.Email);

            if (usuario == null)
            {
                response.HasError = true;
                response.Error = $"Ninguna cuenta registrada con el correo {request.Email}.";
                return response;
            }

            var verificationURL = await _emailHelper.ForgotPasswordURL(usuario, origin);

            await _emailService.SendEmailAsync(new Infraestructure.Dtos.EmailRequest()
            {
                To = usuario.Email,
                Body = $"Por favor, reinicia tu cuenta visitando esta URL {verificationURL}",
                Subject = "Restablecimiento de contraseña"
            });

            return response;
        }

        public async Task<RegisterResponse> RegisterIdentityAsync(RegisterRequest request, string origin)
        {
            RegisterResponse response = new();

            RegisterResponse SetError(string errorMessage)
            {
                response.HasError = true;
                response.Error = errorMessage;
                return response;
            }

            if (await _userManager.FindByNameAsync(request.UserName) != null)
                return SetError($"El nombre de usuario {request.UserName} ya existe, por favor elija otro.");

            if (await _userManager.FindByEmailAsync(request.Email) != null)
                return SetError($"El correo {request.Email} ya existe.");

            if (await _userManager.FindByNameAsync(request.Cedula) != null)
                return SetError($"Ya existe un usuario con la cédula {request.Cedula}.");

            var activeByDefaultRoles = new List<string>
            {
                Roles.Desarrollador.ToString(),
                Roles.Administrador.ToString()
            };

            var usuario = new ApplicationUser
            {
                UserName = request.UserName,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Foto = request.Foto,
                Cedula = request.Cedula,
                Email = request.Email,
                PhoneNumber = request.Phone,
                IsActive = activeByDefaultRoles.Contains(request.Rol),
            };

            if(request.Rol == "Desarrollador" || request.Rol == "Administrador")
            {
                usuario.IsActive = true;
                usuario.EmailConfirmed = true;
            }

            var result = await _userManager.CreateAsync(usuario, request.Password);
            if (!result.Succeeded)
                return SetError(string.Join(", ", result.Errors.Select(e => e.Description)));

            switch (request.Rol)
            {
                case "Administrador":
                    await _userManager.AddToRoleAsync(usuario, Roles.Administrador.ToString());

                    // Opcional: Enviar email de bienvenida a desarrolladores
                    await _emailService.SendEmailAsync(new Infraestructure.Dtos.EmailRequest
                    {
                        To = usuario.Email,
                        Body = $"Bienvenido {usuario.Nombre} como Administrador. Su cuenta ha sido activada automáticamente.",
                        Subject = "Registro de Administrador"
                    });
                    break;

                case "Desarrollador":
                    await _userManager.AddToRoleAsync(usuario, Roles.Desarrollador.ToString());

                    // Opcional: Enviar email de bienvenida a desarrolladores
                    await _emailService.SendEmailAsync(new Infraestructure.Dtos.EmailRequest
                    {
                        To = usuario.Email,
                        Body = $"Bienvenido {usuario.Nombre} como Desarrollador. Su cuenta ha sido activada automáticamente.",
                        Subject = "Registro de Desarrollador"
                    });
                    break;

                case "Agente":
                    await _userManager.AddToRoleAsync(usuario, ClienteAgente.Agente.ToString());
                    await _emailService.SendEmailAsync(new Infraestructure.Dtos.EmailRequest
                    {
                        To = usuario.Email,
                        Body = $"Bienvenido {usuario.Nombre} como Agente Inmobiliario. Su cuenta necesita la activacion. Por favor, contactar con un administrador.",
                        Subject = "Registro de Agente"
                    });
                    break;

                case "Cliente":
                    await _userManager.AddToRoleAsync(usuario, ClienteAgente.Cliente.ToString());
                    var verificacionURL = await _emailHelper.VerificationEmailURL(usuario, origin);
                    await _emailService.SendEmailAsync(new Infraestructure.Dtos.EmailRequest
                    {
                        To = usuario.Email,
                        Body = $"Por favor, confirme su cuenta ingresando a esta URL: {verificacionURL}",
                        Subject = "Registro de confirmación"
                    });
                    break;

                case "Desarrollador":
                    await _userManager.AddToRoleAsync(usuario, Roles.Desarrollador.ToString());
                    break;

                case "Administrador":
                    await _userManager.AddToRoleAsync(usuario, Roles.Administrador.ToString());
                    break;

                default:
                    return SetError("Rol no válido.");
            }

            return response;
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            ResetPasswordResponse response = new();

            ResetPasswordResponse SetError(string errorMessage)
            {
                response.HasError = true;
                response.Error = errorMessage;
                return response;
            }

            if (string.IsNullOrEmpty(request.Email) ||
                string.IsNullOrEmpty(request.Password) ||
                string.IsNullOrEmpty(request.ConfirmPassword))
            {
                return SetError("Por favor llenar todos los campos.");
            }

            var usuario = await _userManager.FindByEmailAsync(request.Email);
            if (usuario == null)
                return SetError($"No hay cuentas registradas con el correo: {request.Email}");

            request.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));

            var result = await _userManager.ResetPasswordAsync(usuario, request.Token, request.Password);
            if (!result.Succeeded)
                return SetError("Ha ocurrido un error al restablecer la contraseña.");
            
            return response;
        }
    }
}
