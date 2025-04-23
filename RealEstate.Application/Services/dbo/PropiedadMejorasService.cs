using AutoMapper;
using Microsoft.Extensions.Logging;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Core;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;

namespace RealEstate.Application.Services.dbo
{
    public class PropiedadMejorasService : IPropiedadMejorasService
    {
        private readonly IPropiedadMejorasRepository _propiedadMejorasRepository;
        private readonly ILogger<MejorasService> _logger;
        private readonly IMapper _mapper;

        public PropiedadMejorasService(IPropiedadMejorasRepository propiedadMejorasRepository,
                              ILogger<MejorasService> logger,
                              IMapper mapper)
        {
            _propiedadMejorasRepository = propiedadMejorasRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _propiedadMejorasRepository.GetAll();

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
                response.Messages = "Ha ocurrido un error obteniendo las mejoras.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> GetByIDAsync(int id)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _propiedadMejorasRepository.GetById(id);

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
                response.Messages = "Ha ocurrido un error obteniendo la mejora.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> RemoveAsync(PropiedadMejorasDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                PropiedadMejoras propiedadMejoras = new PropiedadMejoras();

                propiedadMejoras.PropiedadMejoraID = dto.PropiedadMejoraID;
                var result = await _propiedadMejorasRepository.Remove(propiedadMejoras);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error eliminando la relacion.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> SaveAsync(PropiedadMejorasDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var propiedadMejora = _mapper.Map<PropiedadMejoras>(dto);
                var result = await _propiedadMejorasRepository.Save(propiedadMejora);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error guardando la relacion.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> UpdateAsync(PropiedadMejorasDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var resultGetBy = await _propiedadMejorasRepository.GetById(dto.PropiedadMejoraID);

                if (!resultGetBy.Success)
                {
                    resultGetBy.Success = response.IsSuccess;
                    resultGetBy.Message = response.Messages;

                    return response;
                }

                var mejoras = _mapper.Map<PropiedadMejoras>(dto);
                var result = await _propiedadMejorasRepository.Update(mejoras);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error actualizando la relacion.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }
    }
}
