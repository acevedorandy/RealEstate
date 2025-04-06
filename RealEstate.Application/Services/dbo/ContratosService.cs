

using AutoMapper;
using Microsoft.Extensions.Logging;
using RealEstate.Application.Contracts.dbo;
using RealEstate.Application.Core;
using RealEstate.Application.Dtos.dbo;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;

namespace RealEstate.Application.Services.dbo
{
    public class ContratosService : IContratosService
    {
        private readonly IContratosRepository _contratosRepository;
        private readonly ILogger<ContratosService> _logger;
        private readonly IMapper _mapper;

        public ContratosService(IContratosRepository contratosRepository,
                                ILogger<ContratosService> logger,
                                IMapper mapper)
        {
            _contratosRepository = contratosRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _contratosRepository.GetAll();

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
                response.Messages = "Ha ocurrido un error obteniendo los contratos.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> GetByIDAsync(int id)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var result = await _contratosRepository.GetById(id);

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
                response.Messages = "Ha ocurrido un error obteniendo el contrato.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> RemoveAsync(ContratosDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                Contratos contratos = new Contratos();

                contratos.ContratoID = dto.ContratoID;
                var result = await _contratosRepository.Remove(contratos);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error elimninando el contrato.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> SaveAsync(ContratosDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var contrato = _mapper.Map<Contratos>(dto);
                var result = await _contratosRepository.Save(contrato);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error agregando el contrato.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }

        public async Task<ServiceResponse> UpdateAsync(ContratosDto dto)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var resultGetBy = await _contratosRepository.GetById(dto.ContratoID);
                
                if (!resultGetBy.Success)
                {
                    response.IsSuccess = resultGetBy.Success;
                    response.Messages = resultGetBy.Message;

                    return response;
                }

                var contrato = _mapper.Map<Contratos>(dto);
                var result = await _contratosRepository.Update(contrato);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = "Ha ocurrido un error actualizando el contrato.";
                _logger.LogError(response.Messages, ex.ToString());
            }
            return response;
        }
    }
}
