using AutoMapper;
using Microsoft.Extensions.Logging;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Core;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;

namespace RealEstate.Application.Services.dbo
{
    public class TiposPropiedadService : ITiposPropiedadService
    {
        private readonly ITiposPropiedadRepository _tiposPropiedadRepository;
        private readonly ILogger<TiposPropiedadService> _logger;
        private readonly IMapper _mapper;

        public TiposPropiedadService(ITiposPropiedadRepository tiposPropiedadRepository,
                                     ILogger<TiposPropiedadService> logger,
                                     IMapper mapper)
        {
            _tiposPropiedadRepository = tiposPropiedadRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _tiposPropiedadRepository.GetAll();

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
                response.Messages = "Ha ocurrido un error obteniendo los tipos de propiedades.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> GetByIDAsync(int id)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _tiposPropiedadRepository.GetById(id);

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
                response.Messages = "Ha ocurrido un error obteniendo el tipo de propiedad.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> RemoveAsync(TiposPropiedadDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                TiposPropiedad tiposPropiedad = new TiposPropiedad();

                tiposPropiedad.TipoPropiedadID = dto.TipoPropiedadID;
                var result = await _tiposPropiedadRepository.Remove(tiposPropiedad);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error eliminando la el tipo de propiedad.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> SaveAsync(TiposPropiedadDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var tiposPropiedad = _mapper.Map<TiposPropiedad>(dto);
                var result = await _tiposPropiedadRepository.Save(tiposPropiedad);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error guardando el tipo de propiedad.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> UpdateAsync(TiposPropiedadDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var resultGetBy = await _tiposPropiedadRepository.GetById(dto.TipoPropiedadID);

                if (!resultGetBy.Success)
                {
                    resultGetBy.Success = response.IsSuccess;
                    resultGetBy.Message = response.Messages;

                    return response;
                }

                var tiposPropiedad = _mapper.Map<TiposPropiedad>(dto);
                var result = await _tiposPropiedadRepository.Update(tiposPropiedad);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error actualizando el tipo de propiedad.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }
    }
}
