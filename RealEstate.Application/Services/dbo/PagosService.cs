using AutoMapper;
using Microsoft.Extensions.Logging;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Core;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;

namespace RealEstate.Application.Services.dbo
{
    public class PagosService : IPagosService
    {
        private readonly IPagosRepository _pagosRepository;
        private readonly ILogger<PagosService> _logger;
        private readonly IMapper _mapper;

        public PagosService(IPagosRepository pagosRepository,
                            ILogger<PagosService> logger,
                            IMapper mapper)
        {
            _pagosRepository = pagosRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _pagosRepository.GetAll();

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
                response.Messages = "Ha ocurrido un error obteniendo los pagos.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> GetByIDAsync(int id)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _pagosRepository.GetById(id);

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
                response.Messages = "Ha ocurrido un error obteniendo el pago.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> RemoveAsync(PagosDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                Pagos pagos = new Pagos();

                pagos.PagoID = dto.PagoID;
                var result = await _pagosRepository.Remove(pagos);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error eliminando el pago.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> SaveAsync(PagosDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var pago = _mapper.Map<Pagos>(dto);
                var result = await _pagosRepository.Save(pago);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error guardando el pago.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> UpdateAsync(PagosDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var resultGetBy = await _pagosRepository.GetById(dto.PagoID);

                if (!resultGetBy.Success)
                {
                    resultGetBy.Success = response.IsSuccess;
                    resultGetBy.Message = response.Messages;

                    return response;
                }

                var pago = _mapper.Map<Pagos>(dto);
                var result = await _pagosRepository.Update(pago);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error actualizando el pago.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }
    }
}
