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
    public class MejorasService : IMejorasService
    {
        private readonly IMejorasRepository _mejorasRepository;
        private readonly ILogger<MejorasService> _logger;
        private readonly IMapper _mapper;

        public MejorasService(IMejorasRepository mejorasRepository,
                              ILogger<MejorasService> logger,
                              IMapper mapper)
        {
            _mejorasRepository = mejorasRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> ExisteMejoraAsync(int mejoraId, int propiedadId)
        {
            var result = await _mejorasRepository.ExisteMejora(mejoraId, propiedadId);
            return result;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _mejorasRepository.GetAll();

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
                var result = await _mejorasRepository.GetById(id);

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

        public async Task<ServiceResponse> GetMejorasByPropertyAsync(int propiedadId)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _mejorasRepository.GetMejorasByProperty(propiedadId);

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

        public async Task<ServiceResponse> RemoveAsync(MejorasDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                Mejoras mejoras = new Mejoras();

                mejoras.MejoraID = dto.MejoraID;
                var result = await _mejorasRepository.Remove(mejoras);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error eliminando la mejora.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> SaveAsync(MejorasDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var mejoras = _mapper.Map<Mejoras>(dto);
                var result = await _mejorasRepository.Save(mejoras);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error guardando la mejora.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> UpdateAsync(MejorasDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var resultGetBy = await _mejorasRepository.GetById(dto.MejoraID);

                if (!resultGetBy.Success)
                {
                    resultGetBy.Success = response.IsSuccess;
                    resultGetBy.Message = response.Messages;

                    return response;
                }

                var mejoras = _mapper.Map<Mejoras>(dto);
                var result = await _mejorasRepository.Update(mejoras);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error actualizando la mejora.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }
    }
}
