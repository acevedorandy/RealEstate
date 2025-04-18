using AutoMapper;
using Microsoft.Extensions.Logging;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Core;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;

namespace RealEstate.Application.Services.dbo
{
    public class PropiedadFotosService : IPropiedadFotosService
    {
        private readonly IPropiedadFotosRepository _propiedadFotosRepository;
        private readonly ILogger<PropiedadFotosService> _logger;
        private readonly IMapper _mapper;

        public PropiedadFotosService(IPropiedadFotosRepository propiedadFotosRepository, 
                                     ILogger<PropiedadFotosService> logger,
                                     IMapper mapper)
        {
            _propiedadFotosRepository = propiedadFotosRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> AddPhotoAsEntity(int propiedadId, List<string> imagePaths)
        {
            ServiceResponse response = new();

            try
            {
                foreach (var imagePath in imagePaths)
                {
                    var fotoDto = new PropiedadFotosDto
                    {
                        PropiedadID = propiedadId,
                        Imagen = imagePath
                    };

                    var fotoEntity = _mapper.Map<PropiedadFotos>(fotoDto);
                    var result = await _propiedadFotosRepository.Save(fotoEntity);

                    if (!result.Success)
                    {
                        response.Messages += $"Error al guardar foto {imagePath}. ";
                        _logger.LogWarning($"Error al guardar foto {imagePath}: {result.Message}");
                    }
                }

                response.IsSuccess = string.IsNullOrEmpty(response.Messages);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Error interno al guardar las fotos";
                _logger.LogError(ex, response.Messages);
            }

            return response;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _propiedadFotosRepository.GetAll();

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
                response.Messages = "Ha ocurrido un error obteniendo las relaciones.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> GetByIDAsync(int id)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _propiedadFotosRepository.GetById(id);

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
                response.Messages = "Ha ocurrido un error obteniendo la relacion.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> GetPhotosByPropertyAsync(int propiedadId)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _propiedadFotosRepository.GetPhotosByProperty(propiedadId);

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
                response.Messages = "Ha ocurrido un error obteniendo las fotos.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> RemoveAsync(PropiedadFotosDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                PropiedadFotos propiedadFotos = new PropiedadFotos();

                propiedadFotos.RelacionID = dto.RelacionID;
                var result = await _propiedadFotosRepository.Remove(propiedadFotos);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error eliminando la relacion.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> SaveAsync(PropiedadFotosDto dto)
        {
            ServiceResponse response = new ServiceResponse();
            
            try
            {
                var propiedad = _mapper.Map<PropiedadFotos>(dto);
                var result = await _propiedadFotosRepository.Save(propiedad);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error guardadndo la relacion.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> UpdateAsync(PropiedadFotosDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var resultGetBy = await _propiedadFotosRepository.GetById(dto.RelacionID);

                if (!resultGetBy.Success)
                {
                    resultGetBy.Success = response.IsSuccess;
                    resultGetBy.Message = response.Messages;

                    return response;
                }

                var propiedad = _mapper.Map<PropiedadFotos>(dto);
                var result = await _propiedadFotosRepository.Update(propiedad);
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
