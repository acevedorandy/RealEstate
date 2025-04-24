using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Contracts.identity;
using RealEstate.Application.Core;
using RealEstate.Application.Dtos.identity;
using RealEstate.Application.Dtos.identity.account;
using RealEstate.Application.Responses.identity;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;
using RealEstate.Persistance.Models.ViewModel;
using RealEstate.Application.Helpers.web;
using RealEstate.Application.Dtos.dbo;
using Microsoft.AspNetCore.Identity;
using RealEstate.Identity.Shared.Entities;
using RealEstate.Application.Models;

namespace RealEstate.Application.Services.dbo
{
    public class UsuariosService : IUsuariosService
    {
        private readonly IAccountServiceForWebApp _accountService;
        private readonly IPropiedadesRepository _propiedadesRepository;
        private readonly IMapper _mapper;
        private readonly IUsuariosRepository _usuariosRepository;
        private readonly ILogger<UsuariosService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse authentication;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsuariosService(IAccountServiceForWebApp accountService,
                               IPropiedadesRepository propiedadesRepository,
                               IMapper mapper,
                               IUsuariosRepository usuariosRepository,
                               ILogger<UsuariosService> logger,
                               IHttpContextAccessor httpContextAccessor,
                               UserManager<ApplicationUser> userManager)
        {
            _accountService = accountService;
            _mapper = mapper;
            _usuariosRepository = usuariosRepository;
            _logger = logger;
            _propiedadesRepository = propiedadesRepository;
            _httpContextAccessor = httpContextAccessor;
            authentication = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("usuario");
            _userManager = userManager;
        }

        public async Task<ServiceResponse> ActivarOrDesactivarAsync(string userId)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                if (userId == authentication.Id)
                {
                    response.IsSuccess = false;
                    response.Messages = "Usted no puede activar o desactivar su propio usuario.";
                    return response;
                }
                var result = await _usuariosRepository.ActivarOrDesactivar(userId);

                if (!result.Success)
                {
                    response.IsSuccess = result.Success;
                    response.Messages = response.Messages;
                    return response;
                }

                response.Model = result.Data;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error actualiando el estado del usuario.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> GetAgentActiveAsync()
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _usuariosRepository.GetAgentActive();

                if (!result.Success)
                {
                    result.Success = response.IsSuccess;
                    result.Message = response.Messages;

                    return response;
                }
                response.Model = result.Data;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error obteniendo los agentes activos.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }
        public async Task<ServiceResponse> GetAgentByNameAsync(string name)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _usuariosRepository.GetAgentByName(name);

                if (!result.Success)
                {
                    result.Success = response.IsSuccess;
                    result.Message = response.Messages;

                    return response;
                }
                response.Model = result.Data;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error obteniendo el agente.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> GetIdentityUserAllAsync()
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _usuariosRepository.GetIdentityUserAll();

                if (!result.Success)
                {
                    result.Success = response.IsSuccess;
                    result.Message = response.Messages;

                    return response;
                }
                response.Model = result.Data;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error obteniendo los usuarios.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> GetIdentityUserByAsync(string userId)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _usuariosRepository.GetIdentityUserBy(userId);

                if (!result.Success)
                {
                    result.Success = response.IsSuccess;
                    result.Message = response.Messages;

                    return response;
                }
                response.Model = result.Data;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error obteniendo el usuario.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }
        public async Task<ServiceResponse> LoadHomeView()
        {
            ServiceResponse response = new ServiceResponse();

            ServiceResponse SetError(string errorMessage)
            {
                response.IsSuccess = false;
                response.Messages = errorMessage;
                return response;
            }

            try
            {
                var propiedades = await _propiedadesRepository.GetAll();

                if (!propiedades.Success)
                    return SetError($"Ha ocurrido un error.");

                var propiedadesData = propiedades.Data as List<PropiedadesModel>;

                var usuarios = await _usuariosRepository.GetIdentityUserAll();

                if (!usuarios.Success)
                    return SetError($"Ha ocurrido un error.");

                var usuariosData = usuarios.Data as List<UsuariosModel>;

                HomeAdmin homeAdmin = new HomeAdmin
                {
                    UsuariosModelView = usuariosData,
                    PropiedadesModelView = propiedadesData
                };

                response.Model = homeAdmin;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un procesando la solicitud.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }
        public async Task<ServiceResponse> GetAllAgentAsync()
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _usuariosRepository.GetAllAgent();

                if (!result.Success)
                {
                    result.Success = response.IsSuccess;
                    result.Message = response.Messages;

                    return response;
                }
                response.Model = result.Data;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error obteniendo los agentes.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> GetAllDeveloperAsync()
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _usuariosRepository.GetAllDeveloper();

                if (!result.Success)
                {
                    result.Success = response.IsSuccess;
                    result.Message = response.Messages;

                    return response;
                }
                response.Model = result.Data;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error obteniendo los desarrolladores.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> GetAllAdminsAsync()
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _usuariosRepository.GetAllAdmins();

                if (!result.Success)
                {
                    result.Success = response.IsSuccess;
                    result.Message = response.Messages;

                    return response;
                }
                response.Model = result.Data;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error obteniendo los desarrolladores.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> RemoveAgentWithPropertyAsync(string userId)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _usuariosRepository.RemoveAgentWithProperty(userId);

                if (!result.Success)
                {
                    result.Success = response.IsSuccess;
                    result.Message = response.Messages;

                    return response;
                }
                response.Model = result.Data;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error procesando la solicitud.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> UpdateIdentityUserAsync(UsuariosDto user)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var resultGetBy = await _usuariosRepository.GetIdentityUserBy(user.Id);

                if (!resultGetBy.Success)
                {
                    resultGetBy.Success = response.IsSuccess;
                    resultGetBy.Message = response.Messages;

                    return response;
                }

                var usuario = _mapper.Map<ApplicationUser>(user);
                var result = await _usuariosRepository.UpdateIdentityUser(usuario);

                if (result.Success)
                {
                    response.Model = _mapper.Map<UsuariosDto>(result.Data);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Messages = result.Message;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error actualizando el usuario.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> UpdatePhotoIdentityUserAsync(string Id, string foto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _usuariosRepository.UpdatePhotoIdentityUser(Id, foto);

                if (!result.Success)
                {
                    result.Success = response.IsSuccess;
                    result.Message = response.Messages;

                    return response;
                }
                response.Model = result.Data;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error actualizando la foto del usuario.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> GetPerfilInformation(string id)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _usuariosRepository.GetIdentityUserBy(id);

                if (!result.Success)
                {
                    result.Success = response.IsSuccess;
                    result.Message = response.Messages;

                    return response;
                }

                var data = result.Data as UsuariosModel;

                PerfilModel modelo = new PerfilModel
                {
                    Id = data.Id,
                    Nombre = data.Nombre,
                    Apellido = data.Apellido,
                    PhoneNumber = data.Telefono,
                    Cedula = data.Cedula,
                    Foto = data.Foto,
                    Email = data.Email,
                };

                response.Model = modelo;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error obteniendo el usuario.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        /* Metodos de las cuentas */
        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {
            return await _accountService.ConfirmAccountAsync(userId, token);
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto, string origin)
        {
            ForgotPasswordRequest forgotPassword = _mapper.Map<ForgotPasswordRequest>(forgotPasswordDto);
            return await _accountService.ForgotPasswordAsync(forgotPassword, origin);
        }

        public async Task<AuthenticationResponse> LoginAsync(LoginDto loginDto)
        {
            AuthenticationRequest authentication = _mapper.Map<AuthenticationRequest>(loginDto);
            AuthenticationResponse response = await _accountService.AuthenticateAsync(authentication);
            return response;
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterDto registerDto, string origin)
        {
            RegisterRequest register = _mapper.Map<RegisterRequest>(registerDto);
            register.FotoFile = registerDto.File; 

            RegisterResponse response = await _accountService.RegisterIdentityAsync(register, origin);

            return response;
        }


        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            ResetPasswordRequest passwordRequest = _mapper.Map<ResetPasswordRequest>(resetPasswordDto);
            return await _accountService.ResetPasswordAsync(passwordRequest);
        }

        public async Task SignOutAsync()
        {
            await _accountService.SignOutAsync();
        }
    }
}
