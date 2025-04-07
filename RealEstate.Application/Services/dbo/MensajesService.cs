
using AutoMapper;
using Microsoft.Extensions.Logging;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Core;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Repositories.dbo;

namespace RealEstate.Application.Services.dbo
{
    public class MensajesService : IMensajesService
    {
        private readonly IMensajesRepository _mensajesRepository;
        private readonly ILogger<MensajesService> _logger;
        private readonly IMapper _mapper;

        public MensajesService(IMensajesRepository mensajesRepository,
                               ILogger<MensajesService> logger,
                               IMapper mapper )
        {
            _mensajesRepository = mensajesRepository;
            _logger = logger;
            _mapper = mapper;
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
