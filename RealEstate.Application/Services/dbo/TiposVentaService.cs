using AutoMapper;
using Microsoft.Extensions.Logging;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Core;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;

namespace RealEstate.Application.Services.dbo
{
    public class TiposVentaService : ITiposVentaService
    {
        private readonly ITiposVentaRepository _tiposVentaRepository;
        private readonly ILogger<TiposVentaService> _logger;
        private readonly IMapper _mapper;

        public TiposVentaService(ITiposVentaRepository tiposVentaRepository,
                                 ILogger<TiposVentaService> logger,
                                 IMapper mapper)
        {
            _tiposVentaRepository = tiposVentaRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _tiposVentaRepository.GetAll();

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
                response.Messages = "Ha ocurrido un error obteniendo los tipos de ventas.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> GetByIDAsync(int id)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _tiposVentaRepository.GetById(id);

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
                response.Messages = "Ha ocurrido un error obteniendo el tipo de venta.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> RemoveAsync(TiposVentaDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                TiposVenta tiposVenta = new TiposVenta();

                tiposVenta.TipoVentaID = dto.TipoVentaID;
                var result = await _tiposVentaRepository.Remove(tiposVenta);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error eliminando el tipo de venta.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> RemoveTypeSalesWithPropertyAsync(int tipoId)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _tiposVentaRepository.RemoveTypeSalesWithProperty(tipoId);

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
                response.Messages = "Ha ocurrido un error eliminando el tipo de venta junto a sus propiedades asociadas.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> SaveAsync(TiposVentaDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var tipoVenta = _mapper.Map<TiposVenta>(dto);
                var result = await _tiposVentaRepository.Save(tipoVenta);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error guardando el tipo de venta.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> UpdateAsync(TiposVentaDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var resultGetBy = await _tiposVentaRepository.GetById(dto.TipoVentaID);

                if (!resultGetBy.Success)
                {
                    resultGetBy.Success = response.IsSuccess;
                    resultGetBy.Message = response.Messages;

                    return response;
                }

                var tipoVenta = _mapper.Map<TiposVenta>(dto);
                var result = await _tiposVentaRepository.Update(tipoVenta);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error actualizando el tipo de venta.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }
    }
}
