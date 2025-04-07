using AutoMapper;
using Microsoft.Extensions.Logging;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Core;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;

namespace RealEstate.Application.Services.dbo
{
    public class PropiedadesService : IPropiedadesService
    {
        private readonly IPropiedadesRepository _propiedadesRepository;
        private readonly ILogger<PropiedadesService> _logger;
        private readonly IMapper _mapper;

        public PropiedadesService(IPropiedadesRepository propiedadesRepository,
                                  ILogger<PropiedadesService> logger,
                                  IMapper mapper)
        {
            _propiedadesRepository = propiedadesRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _propiedadesRepository.GetAll();

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
                response.Messages = "Ha ocurrido un error obteniendo las propiedades.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> GetByIDAsync(int id)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _propiedadesRepository.GetById(id);

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
                response.Messages = "Ha ocurrido un error obteniendo la propiedad.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> RemoveAsync(PropiedadesDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                Propiedades propiedades = new Propiedades();

                propiedades.PropiedadID = dto.PropiedadID;
                var result = await _propiedadesRepository.Remove(propiedades);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error eliminando la propiedad.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> SaveAsync(PropiedadesDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var propiedad = _mapper.Map<Propiedades>(dto);
                var result = await _propiedadesRepository.Save(propiedad);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error guardando la propiedad.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> UpdateAsync(PropiedadesDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var resultGetBy = await _propiedadesRepository.GetById(dto.PropiedadID);

                if (!resultGetBy.Success)
                {
                    resultGetBy.Success = response.IsSuccess;
                    resultGetBy.Message = response.Messages;

                    return response;
                }

                var propiedad = _mapper.Map<Propiedades>(dto);
                var result = await _propiedadesRepository.Update(propiedad);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error actualizando la propiedad.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }
    }
}
