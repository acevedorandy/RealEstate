using AutoMapper;
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

namespace RealEstate.Application.Services.dbo
{
    public class UsuariosService : IUsuariosService
    {
        private readonly IAccountService _accountService;
        private readonly IPropiedadesRepository _propiedadesRepository;
        private readonly IMapper _mapper;
        private readonly IUsuariosRepository _usuariosRepository;
        private readonly ILogger<UsuariosService> _logger;

        public UsuariosService(IAccountService accountService,
                               IPropiedadesRepository propiedadesRepository,
                               IMapper mapper,
                               IUsuariosRepository usuariosRepository,
                               ILogger<UsuariosService> logger)
        {
            _accountService = accountService;
            _mapper = mapper;
            _usuariosRepository = usuariosRepository;
            _logger = logger;
            _propiedadesRepository = propiedadesRepository;
        }

        public Task<ServiceResponse> ActivarOrDesactivarAsync(string userId)
        {
            throw new NotImplementedException();
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

        public Task<ServiceResponse> GetUserByRolAsync(string rol)
        {
            throw new NotImplementedException();
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
