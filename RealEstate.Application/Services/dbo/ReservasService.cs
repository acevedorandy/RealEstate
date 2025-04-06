using AutoMapper;
using Microsoft.Extensions.Logging;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Core;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;

namespace RealEstate.Application.Services.dbo
{
    public class ReservasService : IReservasService
    {
        private readonly IReservasRepository _reservasRepository;
        private readonly ILogger<ReservasService> _logger;
        private readonly IMapper _mapper;

        public ReservasService(IReservasRepository reservasRepository,
                               ILogger<ReservasService> logger,
                               IMapper mapper) 
        {
            _reservasRepository = reservasRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _reservasRepository.GetAll();

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
                response.Messages = "Ha ocurrido un error obteniendo las reservas.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> GetByIDAsync(int id)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _reservasRepository.GetById(id);

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
                response.Messages = "Ha ocurrido un error obteniendo la reserva.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> RemoveAsync(ReservasDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                Reservas reservas = new Reservas();

                reservas.ReservaID = dto.ReservaID;
                var result = await _reservasRepository.Remove(reservas);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error eliminando la reserva.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> SaveAsync(ReservasDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var reserva = _mapper.Map<Reservas>(dto);
                var result = await _reservasRepository.Save(reserva);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error guardando la reserva.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> UpdateAsync(ReservasDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var resultGetBy = await _reservasRepository.GetById(dto.ReservaID);

                if (!resultGetBy.Success)
                {
                    resultGetBy.Success = response.IsSuccess;
                    resultGetBy.Message = response.Messages;

                    return response;
                }

                var reserva = _mapper.Map<Reservas>(dto);
                var result = await _reservasRepository.Update(reserva);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error actualizando la reserva.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }
    }
}
