using AutoMapper;
using Microsoft.Extensions.Logging;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Core;
using RealEstate.Application.Dtos;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;

namespace RealEstate.Application.Services.dbo
{
    public class PropiedadTiposVentaService : IPropiedadTiposVentaService
    {
        private readonly IPropiedadTiposVentaRepository _propiedadTiposVentaRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PropiedadTiposVentaService> _logger;

        public PropiedadTiposVentaService(IPropiedadTiposVentaRepository propiedadTiposVentaRepository,
                                          IMapper mapper,
                                          ILogger<PropiedadTiposVentaService> logger)
        {
            _propiedadTiposVentaRepository = propiedadTiposVentaRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _propiedadTiposVentaRepository.GetAll();

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
                var result = await _propiedadTiposVentaRepository.GetById(id);

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

        public async Task<ServiceResponse> RemoveAsync(PropiedadTiposVentaDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                PropiedadTiposVenta tiposVenta = new PropiedadTiposVenta();

                tiposVenta.PropiedadTipoVentaID = dto.PropiedadTipoVentaID;
                var result = await _propiedadTiposVentaRepository.Remove(tiposVenta);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error eliminando la relacion.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> SaveAsync(PropiedadTiposVentaDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var propiedad = _mapper.Map<PropiedadTiposVenta>(dto);
                var result = await _propiedadTiposVentaRepository.Save(propiedad);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error guardando la relacion.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> UpdateAsync(PropiedadTiposVentaDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var resultGetBy = await _propiedadTiposVentaRepository.GetById(dto.PropiedadTipoVentaID);

                if (!resultGetBy.Success)
                {
                    response.IsSuccess = resultGetBy.Success;
                    response.Messages = resultGetBy.Message;

                    return response;
                }

                var propiedad = _mapper.Map<PropiedadTiposVenta>(dto);
                var result = await _propiedadTiposVentaRepository.Update(propiedad);
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
