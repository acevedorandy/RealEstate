
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Core;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Application.Responses.identity;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Application.Helpers.web;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Application.Services.dbo
{
    public class MensajesService : IMensajesService
    {
        private readonly IMensajesRepository _mensajesRepository;
        private readonly ILogger<MensajesService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse authentication;

        public MensajesService(IMensajesRepository mensajesRepository,
                               ILogger<MensajesService> logger,
                               IMapper mapper,
                               IHttpContextAccessor httpContextAccessor)
        {
            _mensajesRepository = mensajesRepository;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            authentication = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("usuario");
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _mensajesRepository.GetAll();

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
                response.Messages = "Ha ocurrido un error obteniendo los mensajes.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> GetByIDAsync(int id)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _mensajesRepository.GetById(id);

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
                response.Messages = "Ha ocurrido un error obteniendo el mensaje.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> GetConversation(string remitenteId, string destinatarioId)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _mensajesRepository.GetAll();

                if (!result.Success)
                {
                    response.IsSuccess = false;
                    response.Messages = result.Message;
                    return response;
                }

                var mensajes = result.Data as List<MensajesModel>;

                response.Model = mensajes
                    .Where(m =>
                        (m.RemitenteID == remitenteId && m.DestinatarioID == destinatarioId) ||
                        (m.RemitenteID == destinatarioId && m.DestinatarioID == remitenteId))
                    .OrderBy(m => m.Enviado)
                    .ToList();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error obteniendo los mensajes.";
                _logger.LogError(response.Messages, ex.ToString());
            }

            return response;
        }


        public async Task<ServiceResponse> GetDestinatarioAsync()
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                string remitenteId = authentication.Id;

                var result = await _mensajesRepository.GetDestinatario(remitenteId);

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
                response.Messages = "Ha ocurrido un error obteniendo los chats.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> RemoveAsync(MensajesDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                Mensajes mensajes = new Mensajes();
                mensajes.MensajeID = dto.MensajeID;

                var result = await _mensajesRepository.Remove(mensajes);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error elimninando el mensaje.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> SaveAsync(MensajesDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                //dto.RemitenteID = authentication.Id;
                var mensaje = _mapper.Map<Mensajes>(dto);
                var result = await _mensajesRepository.Save(mensaje);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error agregando el mensaje.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> UpdateAsync(MensajesDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var resultGetBy = await _mensajesRepository.GetById(dto.MensajeID);

                if (!resultGetBy.Success)
                {
                    response.IsSuccess = resultGetBy.Success;
                    response.Messages = resultGetBy.Message;

                    return response;
                }

                var mensaje = _mapper.Map<Mensajes>(dto);
                var result = await _mensajesRepository.Update(mensaje);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error actualizando el mensaje.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }
    }
}
